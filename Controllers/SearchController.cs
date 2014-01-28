using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blogger.Models;
using System.Data.SqlClient;

namespace Blogger.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        private BloggerEntities db = new BloggerEntities();

        public ActionResult Index(string keyword)
        {
            keyword = HttpUtility.UrlDecode(keyword);
            var post = from p in db.Posts
                       where p.Name.StartsWith(keyword) || p.Name.EndsWith(keyword) || p.Description.StartsWith(keyword) || p.Description.EndsWith(keyword) 
                       select p;
            return View(post);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
