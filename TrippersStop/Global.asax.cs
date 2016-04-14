using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using TraveLayer.CacheServices;
using Trippism.APIHelper;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Weather;
using TraveLayer.SoapServices.Hotel;
using TraveLayer.SoapServices.Hotel.Sabre;

using TrippismApi.TraveLayer;

namespace TrippismApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register); //Commented today
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //  BundleConfig.RegisterBundles(BundleTable.Bundles);
            //JsConfig.EmitLowercaseUnderscoreNames = true;
            var container = new Container();
            container.RegisterWebApiRequest<IAsyncSabreAPICaller, SabreAPICaller>();
            container.RegisterWebApiRequest<IAsyncWeatherAPICaller, WeatherAPICaller>();
            if (ApiHelper.IsRedisAvailable())
            {
                container.RegisterWebApiRequest<ICacheService, RedisService>();
            }
            else
            {
                container.RegisterWebApiRequest<ICacheService, MemoryCacheService>();
            }
            //container.RegisterWebApiRequest<IDBService, MongoService>();
            container.RegisterWebApiRequest<IAsyncGoogleAPICaller, GoogleAPICaller>();
            container.RegisterWebApiRequest<IAsyncGoogleReverseLookupAPICaller, GoogleReverseLookupAPICaller>();
            container.RegisterWebApiRequest<IAsyncYelpAPICaller, YelpAPICaller>();
            container.RegisterWebApiRequest<IAsyncGooglePlaceAPICaller, GooglePlaceAPICaller>();

            container.RegisterWebApiRequest<IAsyncYouTubeAPICaller, YouTubeAPICaller>();
            container.RegisterWebApiRequest<ITripAdvisorAPIAsyncCaller, TripAdvisorAPICaller>();
            container.RegisterWebApiRequest<ISabreHotel, SabreHotelCaller>();
            
            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            RegisterMap registerMap = new RegisterMap();
            registerMap.RegisterMappingEntities();
            registerMap.RegisterTripAdvisorMapping();
            RegisterMap.RegisterSabreSoapMapping();

            //GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
            // GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
        }
    }
}
