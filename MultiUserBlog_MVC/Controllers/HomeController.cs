using MultiUserBlog_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiUserBlog_MVC.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.BlogPosts = db.BlogPosts.ToList(); 
            return View();
        }

        
        public ActionResult Create()
        {
            return View();
        }


        }
    }
