using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blogger.Models;

namespace Blogger.Controllers
{
    public class CommentsController : Controller
    {
        private BloggerEntities db = new BloggerEntities();

        [ChildActionOnly]
        public ActionResult CommentDetails(int? id, string postName)
        {
            var comment = from c in db.Comments
                          where c.Post.Id == id  && c.Publish == "Yes"
                          select c; 
            return PartialView(comment);
        }

        [ChildActionOnly]
        public ActionResult CommentsCreate()
        {
            return PartialView();
        }

        [ChildActionOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentsCreate(Comment comment, int ? id)
        {
            string ipAddress = string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.UserHostAddress) ? string.Empty : System.Web.HttpContext.Current.Request.UserHostAddress;

            if (ModelState.IsValid)
            {
                comment.PostId = id;
                comment.Location = Location.GetContactDetails(BloggerConstants.LocationFinder + ipAddress);
                comment.IpAddress = ipAddress;
                comment.Publish = "No";
                comment.DateTime = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Name", comment.PostId);
            ViewBag.Message = "Thank u for you comments , Will reviewed by administrator";
            return PartialView(comment);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}