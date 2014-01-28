using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blogger.Models;

namespace Blogger.Controllers
{
    public class HomeController : Controller
    {
        private BloggerEntities db = new BloggerEntities();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            return View();
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
                           orderby d.Key.Year descending
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
                    Count = post.Count(),
                    PostList = post
                };
                setList.Add(set);
            }
            
            return PartialView(setList);
        }

        [ChildActionOnly]
        public ActionResult Comments()
        {
            _id = int.Parse(Request.QueryString["id"]);
            var comment = from p in db.Comments
                          where (p.Publish == "Yes" && p.PostId == _id)
                          select p;
            return PartialView(comment);
        }

        [ChildActionOnly]
        public ActionResult CommentsCreate()
        {
            
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Name");
            return PartialView();
        }

        [ChildActionOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentsCreate(Comment comment)
        {
            string ipAddress = string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.UserHostAddress) ? string.Empty : System.Web.HttpContext.Current.Request.UserHostAddress;
            
            if (ModelState.IsValid)
            {
                comment.Location =  GetContactDetails(BloggerConstants.LocationFinder + ipAddress);
                comment.IpAddress = ipAddress;
                comment.Publish = "No";
                comment.DateTime = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();             
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "Name",comment.PostId);
            ViewBag.Message = "Thank u for you comments , Will reviewed by administrator";
            return PartialView(comment);
        }

        private int _id;
        public int id
        {
            get { return id; }
            set { id = int.Parse(string.IsNullOrEmpty(Request.QueryString["id"])? "0":Request.QueryString["id"]); }
        }

        static string GetContactDetails(string url)
        {
            HttpWebRequest request = null;
            request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            string result = reader.ReadToEnd();
            return result;

        }


        
    }
}
