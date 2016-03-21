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
    ///  To retrieve city pairs of origin and destination airports
    ///  To get the complete list of origin and destination city pairs that we support
    ///  To get destination airport codes that are associated with an origin
    ///  To get origin and destination airport codes in a specific region or country
    /// </summary>
     [GZipCompressionFilter]
    public class CityPairsController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public string SabreAirSearchCityPairsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreAirSearchCityPairsUrl"];
            }
        }
        public string SabreFareRangeCityPairsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreFareRangeCityPairsUrl"];
            }
        }
        public string SabreLowFareForecastCityPairsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreLowFareForecastCityPairsUrl"];
            }
        }
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public CityPairsController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;           
        }
        /// <summary>
        /// Retrieve city pairs that can be passed to applicable Air Shopping
        /// </summary>
        [ResponseType(typeof(CityPairs))] 
        public HttpResponseMessage Get(string type)
        {
            string url=string.Empty;
            switch (type)
            {
                case "AirSearch":
                    url = SabreAirSearchCityPairsUrl;
                    break;
                case "FareRange":
                    url = SabreFareRangeCityPairsUrl;
                    break;
                case "LowFareForecast":
                    url = SabreLowFareForecastCityPairsUrl;
                    break;
                default:
                   return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
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
                OTA_CityPairsLookup cities = new OTA_CityPairsLookup();
                cities = ServiceStackSerializer.DeSerialize<OTA_CityPairsLookup>(result.Response);                
                CityPairs cityPairs = Mapper.Map<OTA_CityPairsLookup, CityPairs>(cities);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, cityPairs);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response); 
        }
    }
}
