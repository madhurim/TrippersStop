using System.Web.Http;
using System.Web.Mvc;

namespace TrippersStop.Areas.Sabre
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
            //context.MapRoute(
            //    "Sabre_default",
            //    "Sabre/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
            context.MapHttpRoute(
              name: "Sabre_default",
              routeTemplate: "Sabre/api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional }
          );
        }
    }
}
