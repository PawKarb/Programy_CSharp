using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KsiegarniaUKW2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "StrinyStatyczne",
                url: "strony/{nazwa}.html",
                defaults: new { Controller = "home", action = "StronyStatyczne" });

            routes.MapRoute(
                name: "KsiazkiList",
                url: "Kategora/{nazwaKategori}",
                defaults: new { Controller = "Ksiazki", action = "Lista" });

            routes.MapRoute(
                name: "KsiazkiSzczegoly",
                url: "ksiazka-{id}.html",
                defaults: new { Controller = "Ksiazki", action = "Szczegoly" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
