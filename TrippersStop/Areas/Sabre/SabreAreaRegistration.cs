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
            context.MapRoute(
    "Sabre_default",
    "Sabre/{controller}/{action}/{id}",
    new { controller = "Destinations", action = "Get", id = UrlParameter.Optional }
);
        }
    }
}
