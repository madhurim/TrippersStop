using ExpressMapper;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using Trippism.APIExtention.Filters;
using TrippismApi.TraveLayer;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// API retrieves a list of destination airport and multi-airport city (MAC) codes that are associated with the theme in the request
    /// </summary>
    [GZipCompressionFilter]
    public class ThemeAirportController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public string SabreThemeAirportUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreThemeAirportUrl"];
            }
        }
        
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public ThemeAirportController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;         
        }
        /// <summary>
        /// To get a reference of destination airport codes that have been associated with a theme
        /// </summary>
        [ResponseType(typeof(ThemeAirport))]
        public HttpResponseMessage Get(string theme)
        {
            string url = string.Format(SabreThemeAirportUrl, theme);
            return GetResponse(url);
        }
        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_ThemeAirportLookup themeAirportLookup = new OTA_ThemeAirportLookup();
                themeAirportLookup = ServiceStackSerializer.DeSerialize<OTA_ThemeAirportLookup>(result.Response);                
                ThemeAirport themeAirport = Mapper.Map<OTA_ThemeAirportLookup, ThemeAirport>(themeAirportLookup);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, themeAirport);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
