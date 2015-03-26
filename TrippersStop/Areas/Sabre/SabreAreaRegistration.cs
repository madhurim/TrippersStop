using System.Web.Http;
using System.Web.Mvc;

namespace TrippismApi.Areas.Sabre
{
    public class SabreAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sabre";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
         //   context.Routes.MapHttpRoute(
         //    name: "Sabre_default",
         //    routeTemplate: "api/Sabre/Destinations/{id}",
         //    defaults: new { controller = "Destinations", id = RouteParameter.Optional }
         //);

  //          context.Routes.MapHttpRoute("Sabre_default",
  //"Sabre/api/{controller}/{id}",
  //new { id = RouteParameter.Optional });

            //context.MapRoute(
            //    "Sabre_default",
            //    "api/Sabre/{controller}/{action}/{id}",
            //    new { aomction = "Index", id = UrlParameter.Optional }
            //);
          //  context.MapHttpRoute(
          //    name: "Sabre_default",
          //    routeTemplate: "Sabre/api/{controller}/{id}",
          //    defaults: new { id = RouteParameter.Optional }
          //);

   //         context.Routes.MapHttpRoute("Sabre_default",
   //"Sabre/api/{controller}/{id}",
   //new { id = RouteParameter.Optional });

            //context.Routes.MapHttpRoute(
            //name: "Sabre_default",
            //routeTemplate: "api/Sabre/{controller}/{id}",
            //defaults: new { area = "Sabre", id = RouteParameter.Optional });
        //    context.Routes.MapMvcAttributeRoutes();

            context.MapRoute(
    "Sabre_default1",
    "Sabre/{controller}/{action}/{id}",
    new { controller = "Destinations", action = "Get", id = UrlParameter.Optional }
);
        }
    }
}
