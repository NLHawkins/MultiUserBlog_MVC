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
        
        public IQueryable<BlogPosts> GetBlogposts()
        {
            string userId = User.Identity.GetUserId();
            var blogposts = db.BlogPosts.Include(b => b.Owner).Where(b => b.OwnerId == userId);
            return blogposts;
        }


        // GET: BlogPost
        public ActionResult Index()
        {
           ViewBag.User = User.Identity.GetUserName();
            ViewBag.OwnerBlogPosts = GetBlogposts();
            return View(db.BlogPosts.Where(p => p.Public == true).ToList());
        }

        // GET: BlogPost/Details/5
        public ActionResult Details(int? id)
        {
                            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPosts blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            
            return View(blogPost);
        }

        // GET: BlogPost/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPost/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,SampleText,Body,Public")] BlogPosts blogPost)
        {
            string userId = User.Identity.GetUserId();
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

        // GET: BlogPost/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPosts blogPost = db.BlogPosts.Find(id);
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

        // GET: BlogPost/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPosts blogPost = db.BlogPosts.Find(id);
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
            BlogPosts blogPost = db.BlogPosts.Find(id);
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
