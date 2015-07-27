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
            container.RegisterWebApiRequest<ICacheService, RedisService>();
            container.RegisterWebApiRequest<IDBService, MongoService>();
            container.RegisterWebApiRequest<IAsyncGoogleAPICaller, GoogleAPICaller>();
            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
            RegisterMappingEntities();
            //GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
           // GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new AreaHttpControllerSelector(GlobalConfiguration.Configuration));
        }

        private void RegisterMappingEntities()
        {
            Mapper.Register<OTA_DestinationFinder, Fares>();
            Mapper.Register<OTA_FareRange, VM.FareRange>();
            Mapper.Register<OTA_TravelSeasonality, VM.TravelSeasonality>();
            Mapper.Register<OTA_LowFareForecast, LowFareForecast>();
            Mapper.Register<TempHigh, TempHighAvg>()
                  .Member(h => h.Avg, m => m.avg);
            Mapper.Register<TempLow, TempLowAvg>()
               .Member(h => h.Avg, m => m.avg);
            Mapper.Register<Trip, TripWeather>()
            .Member(h => h.TempHighAvg, m => m.temp_high)
            .Member(h => h.TempLowAvg, m => m.temp_low)          
            .Member(h => h.CloudCover, m => m.cloud_cover);
            Mapper.Compile();
        }
    }
}