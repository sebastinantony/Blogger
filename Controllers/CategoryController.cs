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
    public class CategoryController : Controller
    {
        private BloggerEntities db = new BloggerEntities();

        public ActionResult Index(int? id, string categoryName)
        {
            var category = db.Categories.Find(id);
            //var post = db.Posts.Find(category.PostId);
            string realTitle = UrlEncoder.ToFriendlyUrl(category.Name);
            string urlTitle = (categoryName ?? "").Trim().ToLower();
            if (realTitle != urlTitle)
            {
                string url = "/Category/" + category.Id + "/" + realTitle;
                return new RedirectResult(url);
            }
            ViewBag.Title = category.Name;
            return View(category.Posts);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}