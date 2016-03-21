using System.Web.Mvc;

namespace Trippism.Areas.GooglePlace
{
    public class GoogleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "YouTube";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              "Google_default",
              "Google/{controller}/{action}/{id}",
              new { controller = "Google", action = "Get", id = UrlParameter.Optional }
             );
        }
    }
}