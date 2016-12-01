using DataLayer;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TraveLayer.CacheServices;
using TrippismApi.TraveLayer;
using TrippismRepositories;

namespace TrippismProfiles
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var container = new Container();
            container.RegisterWebApiRequest<IDBContext, TrippismMongoDBContext>();
            container.RegisterWebApiRequest<IEmailContext, TrippismTemplateMongoDBContext>();
            if (ApiHelper.IsRedisAvailable())
            {
                container.RegisterWebApiRequest<ICacheService, RedisService>();
            }
            else
            {
                container.RegisterWebApiRequest<ICacheService, MemoryCacheService>();
            }
            container.RegisterWebApiRequest<IAnonymousRepository, AnonymousRepository>();
            container.RegisterWebApiRequest<IActivityRepository, ActivityRepository>();
            container.RegisterWebApiRequest<IAuthDetailsRepository, AuthDetailsRepository>();
            container.RegisterWebApiRequest<IEmailTemplateRepository, EmailTemplateRepository>();
            // This is an extension method from the integration package.
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
            ApiHelper.RegisterMappingEntities();
        }
    }
}
