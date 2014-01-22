using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blogger.Models;

namespace Blogger.Controllers
{
    public class HomeController : Controller
    {
        private BloggerEntities db = new BloggerEntities();

        public ActionResult Index(int id=0)
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //var post = from c in db.Posts
            //           where c.Id == id
            //           select c.Name; 
            //return View(post.ToList());

            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [ChildActionOnly]
        public ActionResult CategoryNavigation()
        {
            var category = db.Categories.ToList();
            return PartialView(category);
        }

        [ChildActionOnly]
        public ActionResult PostList()
        {
            var post = db.Posts.ToList();
            return PartialView(post);
        }

        [ChildActionOnly]
        public ActionResult Archives()
        {
            ArchivesSet set;
            List<ArchivesSet> setList = new List<ArchivesSet>();

            var postDate = from p in db.Posts
                           group p by new { Month = p.Date.Month, Year = p.Date.Year } into d
                           select new
                           {
                               Month = d.Key.Month,
                               Year = d.Key.Year
                           };
            

            foreach (var item in postDate)
            {
                IQueryable<Post> post = from p in db.Posts
                                        where (p.Date.Month == item.Month && p.Date.Year == item.Year)
                                        select p;
                set = new ArchivesSet()
                {
                    
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Month),
                    Year = item.Year,
                    PostList = post
                };
                setList.Add(set);
            }
            
            return PartialView(setList);
        }

        [ChildActionOnly]
        public ActionResult Comments()
        {
            int id = int.Parse(Request.QueryString["id"]);
            var comments = from c in db.Comments
                           where c.PostId == id
                           select c;
            return PartialView(comments);
        }

        [ChildActionOnly]
        public ActionResult 
    }
}
