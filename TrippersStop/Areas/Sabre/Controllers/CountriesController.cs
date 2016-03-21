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
    /// Retrieves a list of origin and destination countries.
    /// </summary>
      [GZipCompressionFilter]
    public class CountriesController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public string SabreCountriesUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreCountriesUrl"];
            }
        }
        public string SabrePointOfSaleCountryUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabrePointOfSaleCountryUrl"];
            }
        }
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public CountriesController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;        
        }
        // GET api/countries
        /// <summary>
        /// Retrieves a list of origin and destination countries.
        /// </summary>
        [ResponseType(typeof(Countries))]
        public HttpResponseMessage Get()
        {
            return GetResponse(SabreCountriesUrl);
        }
        /// <summary>
        /// Retrieves a list of origin and destination countries based on point of sale country.
        /// </summary>
        [ResponseType(typeof(Countries))]
        public HttpResponseMessage Get(string pointofsalecountry)
        {
            string url = string.Format(SabrePointOfSaleCountryUrl, pointofsalecountry);
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
                OTA_CountriesLookup cities = new OTA_CountriesLookup();
                cities = ServiceStackSerializer.DeSerialize<OTA_CountriesLookup>(result.Response);                
                Countries countries = Mapper.Map<OTA_CountriesLookup, Countries>(cities);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, countries);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
