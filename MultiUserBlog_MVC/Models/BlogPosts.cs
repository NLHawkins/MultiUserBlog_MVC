using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MultiUserBlog_MVC.Models
{
    public class BlogPosts
    {
        public int Id { get; set; }

        [DisplayName("Post Title")]
        public string Title { get; set; }

        public string SampleText { get; set; }

        [DisplayName("What is your name?")]
        public string Author { get; set; }

        public DateTime Created { get; set; }

        [DisplayName("Write your Post")]
        public string Body { get; set; }

        public bool Public { get; set; }

        public string OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }
    }

   
}