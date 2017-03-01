using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace AnalysisPortal
{
    using AnalysisPortal.Helpers.WebAPI;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.MapRoute("Robots.txt", "robots.txt", new { controller = "Home", action = "Robots" });

            routes.MapRoute("Resources", "resources/{resxFileName}.{culture}.js", new { controller = "Resources", action = "GetResourcesJavaScript" });

            // Commented out because of problem with Json.Net 4.5
            //routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //).RouteHandler = new SessionStateRouteHandler(); // this is needed to make Session available in a controller inheriting from ApiController.

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}