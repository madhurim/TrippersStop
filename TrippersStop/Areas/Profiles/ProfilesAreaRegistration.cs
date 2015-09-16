using System.Web.Mvc;

namespace Trippism.Areas.Profiles
{
    public class ProfilesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Profiles";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Profiles_default",
                "Profiles/{controller}/{action}/{id}",
                  new { controller = "Destinations", action = "Get", id = UrlParameter.Optional }
            );

        }
    }
}