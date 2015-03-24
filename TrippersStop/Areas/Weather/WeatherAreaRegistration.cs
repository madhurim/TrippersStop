using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;


namespace TrippismApi.Areas.Weather
{
    public class WeatherAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Weather";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
//            context.Routes.MapHttpRoute("Weather_default",
//"Weather/api/{controller}/{id}",
//new { id = RouteParameter.Optional });

            //context.Routes.MapHttpRoute(
            //    name: "Weather_default",
            //    routeTemplate: "api/Weather/{id}",
            //    defaults: new { controller = "History", id = RouteParameter.Optional }
            //);

            context.MapRoute(
                "Weather_default1",
                "api/Weather/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
           // context.MapHttpRoute(
           //    name: "Weather_default",
           //    routeTemplate: "Weather/{controller}/{action}/{id}",
           //    defaults: new { id = RouteParameter.Optional }
           //);
                      
        }  
        
    }
}