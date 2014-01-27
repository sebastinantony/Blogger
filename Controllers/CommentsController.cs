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

        //
        // GET: /Comments/

        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Post);
            return View(comments.ToList());
        }

        //
        // GET: /Comments/Details/5

        public ActionResult Details(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // GET: /Comments/Create

        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Name");
            return View();
        }

        //
        // POST: /Comments/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Posts, "Id", "Name", comment.PostId);
            return View(comment);
        }

        //
        // GET: /Comments/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Name", comment.PostId);
            return View(comment);
        }

        //
        // POST: /Comments/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Name", comment.PostId);
            return View(comment);
        }

        //
        // GET: /Comments/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        //
        // POST: /Comments/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [ChildActionOnly]
        public ActionResult CommentDetails(int? id, string postName)
        {
            var comment = from c in db.Comments
                          where c.Post.Id == id  && c.Publish == "Yes"
                          select c; 
            return PartialView(comment);
        }

        [ChildActionOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentsCreate(Comment comment)
        {
            string ipAddress = string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.UserHostAddress) ? string.Empty : System.Web.HttpContext.Current.Request.UserHostAddress;

            if (ModelState.IsValid)
            {
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

    }
}