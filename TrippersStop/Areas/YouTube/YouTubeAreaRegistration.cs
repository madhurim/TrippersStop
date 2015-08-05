using System.Web.Http;
using System.Web.Mvc;

namespace Trippism.Areas.YouTube
{
    public class YouTubeAreaRegistration : AreaRegistration
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
            "YouTube_default",
            "YouTube/{controller}/{action}/{id}",
            new { controller = "YouTube", action = "Get", id = UrlParameter.Optional }
           );
        }
    }
}