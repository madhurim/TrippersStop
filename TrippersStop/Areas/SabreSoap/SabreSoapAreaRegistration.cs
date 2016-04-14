using System.Web.Mvc;

namespace Trippism.Areas.SabreSoap
{
    public class SabreSoapAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SabreSoap";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SabreSoap_default",
                "SabreSoap/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}