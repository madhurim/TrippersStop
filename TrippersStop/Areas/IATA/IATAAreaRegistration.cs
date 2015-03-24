using System.Web.Http;
using System.Web.Mvc;

namespace TrippismApi.Areas.IATA
{
    public class IATAAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IATA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
          //  context.Routes.MapHttpRoute(
          //    name: "IATA_default",
          //    routeTemplate: "api/IATA/{id}",
          //    defaults: new { controller = "IATA", id = RouteParameter.Optional }
          //);

   //context.Routes.MapHttpRoute("IATA_default",
   //"IATA/api/{controller}/{id}",
   //new { id = RouteParameter.Optional });

            context.MapRoute(
                "IATA_default",
                "IATA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            //context.MapHttpRoute(
            //  name: "IATA_default",
            //  routeTemplate: "IATA/{controller}/{action}/{id}",
            //  defaults: new { id = RouteParameter.Optional });

//   context.Routes.MapHttpRoute("IATA_default",
//"IATA/api/{controller}/{id}",
//new { id = RouteParameter.Optional });
        }      
    }
}
