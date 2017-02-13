using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MultiUserBlog_MVC.Models;
using Microsoft.AspNet.Identity;

namespace MultiUserBlog_MVC.Controllers
{
    public class BlogPostController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public IQueryable<BlogPosts> GetOwnerAllPosts()
        {
            string userId = User.Identity.GetUserId();
            var blogposts = db.BlogPosts.Include(b => b.Owner).Where(b => b.OwnerId == userId);
            return blogposts;
        }

        [Authorize]
        public ActionResult Index()
        {
            ViewBag.User = User.Identity.GetUserName();
            ViewBag.OwnerBlogPosts = GetOwnerAllPosts().OrderByDescending(c => c.Created); 
            return View(db.BlogPosts.Where(p => p.Public == true).ToList());
        }

        [Authorize]
        public ActionResult Details(int? id)
        {                            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string you = "You";
            var userId = User.Identity.GetUserId();
            BlogPosts publicPost = new BlogPosts();
            BlogPosts detailPost = db.BlogPosts.
                Where
                    (b => b.Id == id
                    && b.OwnerId == userId)
                    .FirstOrDefault();
            ViewBag.Author = you;
            if(detailPost == null)
            {
                publicPost = db.BlogPosts
                    .Where
                        (b => b.Id == id
                        && b.Public == true)
                        .FirstOrDefault();
                detailPost = publicPost;
                ViewBag.Author = detailPost.Author;

            }
            if (detailPost == null)
            {
                return HttpNotFound();
            }
                      
            return View(detailPost);
        }

        [Authorize]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            if(userId == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        // POST: BlogPost/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,SampleText,Body,Public")] BlogPosts blogPost)
        {
            string userId = User.Identity.GetUserId();
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                if (blogPost.SampleText == null)
                {
                    if (blogPost.Body.Length > 40)
                    {
                        blogPost.SampleText = blogPost.Body.Substring(0, 40) + "...";
                    }

                    else
                    {
                        blogPost.SampleText = blogPost.Body;
                    }
                }
                
                blogPost.Created = DateTime.Now;
                blogPost.OwnerId = User.Identity.GetUserId();
                blogPost.Author = User.Identity.GetUserName();
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userId = User.Identity.GetUserId();
            BlogPosts blogPost = db.BlogPosts.Where(b => b.OwnerId == userId).Where(b => b.Id == id).FirstOrDefault();
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPost/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,SampleText,Author,Created,Body,Public")] BlogPosts blogPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userId = User.Identity.GetUserId();
            BlogPosts blogPost = db.BlogPosts.Where(b => b.OwnerId == userId).Where(b => b.Id == id).FirstOrDefault();
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPost/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string userId = User.Identity.GetUserId();
            BlogPosts blogPost = db.BlogPosts.Where(b => b.OwnerId == userId).Where(b => b.Id == id).FirstOrDefault();            
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
