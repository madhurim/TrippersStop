using System.Web.Mvc;

namespace Trippism.Areas.TripAdvisor
{
    public class TripAdvisorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TripAdvisor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TripAdvisor_default",
                "TripAdvisor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}