using System.Web.Mvc;
using System.Web.Routing;

namespace TrippismApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Please do not uncomment this . In production we need to run the index.html from client side AngularJS code and not this

           /*   routes.MapRoute(
                  name: "Default",
                  url: "{controller}/{action}/{id}",
                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
              );*/
        }
    }
}