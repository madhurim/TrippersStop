using System.Web.Mvc;

namespace Trippism.Areas.CurrencyConversion
{
    public class CurrencyConversionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CurrencyConversion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CurrencyConversion_default",
                "CurrencyConversion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}