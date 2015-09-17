using DataLayer;
using ExpressMapper;
using ServiceStack.Text;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TraveLayer.CacheServices;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Weather;
using TrippismApi.TraveLayer;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;

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
            BundleConfig.RegisterBundles(BundleTable.Bundles);
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

            container.RegisterWebApiRequest<IAsyncYouTubeAPICaller, YouTubeAPICaller>();
            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
            ApiHelper.RegisterMappingEntities();
            //GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
            // GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
        }
    }
}