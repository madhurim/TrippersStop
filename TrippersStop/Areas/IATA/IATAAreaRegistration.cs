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
            context.MapRoute(
                "IATA_default",
                "IATA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
