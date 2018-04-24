using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bookstore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
       
            routes.MapRoute(
                null,
                "BookListPage{Page}",
                new
                {
                    controller = "Book",
                    action = "Pagination",
                    Specilization = (string) null,
                },
                new
                {
                    page = @"\d+"
                }
                );

            routes.MapRoute(
                null,
                "{Specilization}",
                new
                {
                    controller = "Book",
                    action = "Pagination",
                    Page = 1
                }
                );
            routes.MapRoute(
                name: null,
                url: "BookListPage{Page}",
                defaults: new { controller = "Book", action = "Pagination" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Book", action = "Pagination", id = UrlParameter.Optional }
            );
        }
    }
}
