using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blogger
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Post",
                "Post/{id}/{postName}",
                new { controller = "Post", action = "Index", id = UrlParameter.Optional, postName = "" },
                new[] { "Blogger.Controllers" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } ,
                new[] { "Blogger.Controllers" }
            );

            
        }
    }
}