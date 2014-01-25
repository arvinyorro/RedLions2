using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RedLions.Presentation.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Add a ProfileDefault this so that ~/Profile url is not mixed with ~/referrerUsername
            routes.MapRoute(
                name: "ProfileDefault",
                url: "Profile",
                defaults: new { controller = "Profile", action = "Index" },
                namespaces: new[] { "RedLions.Presentation.Web.Controllers" }
            );

            // Enables Attribute Routing
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "RedLions.Presentation.Web.Controllers" }
            );
        }
    }
}