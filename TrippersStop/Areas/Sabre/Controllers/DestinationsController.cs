using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippismApi.TraveLayer;
using System.Text;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Configuration;
using System.Web.Http.Description;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Weather;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using Trippism.APIHelper;
using ExpressMapper;
using Trippism.APIExtention.Filters;
using NLog;
namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// Return the current nonstop lead fare and an overall lead fare available to destinations
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class DestinationsController : ApiController
    {

        IAsyncSabreAPICaller _apiCaller;
        IAsyncWeatherAPICaller _weatherApiCaller;
        ICacheService _cacheService;
        const string _destinationKey = "Trippism.Destinations.All";
        string _expireTime = ConfigurationManager.AppSettings["RedisExpireInMin"].ToString();
        public string SabreDestinationsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreDestinationsUrl"];
            }
        }
        public string SabreFareForecastUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreFareForecastUrl"];
            }
        }
        public string SabreFareRangeUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreFareRangeUrl"];
            }
        }
        public string SabreSeasonalityUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSeasonalityUrl"];
            }
        }

        public string WeatherHistoryUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WeatherHistoryUrl"];
            }
        }
        public string SabreCountriesUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSaleCountryUrl"];
            }
        }


        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public DestinationsController(IAsyncSabreAPICaller apiCaller, IAsyncWeatherAPICaller weatherApiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
            _weatherApiCaller = weatherApiCaller;
        }

        // GET api/DestinationFinder
        /// <summary>
        /// Return the current nonstop lead fare and an overall lead fare available to destinations from a specific origin on roundtrip travel dates 
        /// </summary>
        /// <param name="destinationsRequest">
        /// Return record based on destinations Request Type</param>
        [ResponseType(typeof(Fares))]
        public async Task<HttpResponseMessage> Get([FromUri]Destinations destinationsRequest)
        {
            string url = GetURL(destinationsRequest);
            return await Task.Run(() =>
             { return GetResponse(url); });
        }

        // GET api/DestinationFinder
        /// <summary>
        /// Filters the response for destination airport codes associated with a travel theme
        /// </summary>
        [Route("api/destinations/theme/{theme}")]
        [Route("api/sabre/destinations/theme/{theme}")]
        [HttpGet]
        [ResponseType(typeof(Fares))]
        public async Task<HttpResponseMessage> GetDestinationsByTheme(string theme, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format(SabreDestinationsUrl + "?origin={0}&departuredate={1}&returndate={2}&lengthofstay={3}&theme={4}", origin, departuredate, returndate, lengthofstay, theme);
            return await Task.Run(() =>
            { return GetResponse(url); });
        }
        // GET api/DestinationFinder
        /// <summary>
        /// Return the cheapest detinations 
        /// </summary>
        [Route("api/destinations/cheapest/{count}")]
        [Route("api/sabre/destinations/cheapest/{count}")]
        [HttpGet]
        [ResponseType(typeof(Fares))]
        public async Task<HttpResponseMessage> GetTopCheapestDestinations(int count, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format(SabreDestinationsUrl + "?origin={0}&departuredate={1}&returndate={2}&lengthofstay={3}", origin, departuredate, returndate, lengthofstay);
            return await Task.Run(() =>
            { return GetResponse(url, count); });

        }
        // GET api/DestinationFinder
        /// <summary>
        /// Filters the response for overall lead fares that are equal to or less than the maximum, and returns the value in LowestFare
        /// </summary>
        [Route("api/destinations/maxfare/{maxfare}")]
        [Route("api/sabre/destinations/maxfare/{maxfare}")]
        [HttpGet]
        [ResponseType(typeof(Fares))]
        public async Task<HttpResponseMessage> GetDestinationsByMaxFare(double maxfare, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format(SabreDestinationsUrl + "?origin={0}&departuredate={1}&returndate={2}&maxfare={3}&lengthofstay={4}", origin, departuredate, returndate, maxfare, lengthofstay);
            return await Task.Run(() =>
            { return GetResponse(url); });
        }
        // GET api/DestinationFinder
        /// <summary>
        /// Filters the response for destinations in the country or countries you specify
        /// </summary>
        [Route("api/sabre/destinations/country/{country}")]
        [HttpGet]
        [ResponseType(typeof(Fares))]
        public async Task<HttpResponseMessage> GetDestinationsByCountry(string country, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format(SabreDestinationsUrl + "?origin={0}&departuredate={1}&returndate={2}&maxfare={3}&lengthofstay={4}&location={4}", origin, departuredate, returndate, lengthofstay, country);
            return await Task.Run(() =>
            { return GetResponse(url); });
        }


        /// <summary>
        /// Format url based on request.
        /// </summary>
        private string GetURL(Destinations destinationsRequest)
        {
            StringBuilder url = new StringBuilder();
            url.Append(SabreDestinationsUrl + "?");
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Origin))
            {
                url.Append("origin=" + destinationsRequest.Origin);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Destination))
            {
                url.Append("&destination=" + destinationsRequest.Destination);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.DepartureDate))
            {
                url.Append("&departuredate=" + destinationsRequest.DepartureDate);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.ReturnDate))
            {
                url.Append("&returndate=" + destinationsRequest.ReturnDate);
            }

            if (!string.IsNullOrWhiteSpace(destinationsRequest.Earliestdeparturedate))
            {
                url.Append("&earliestdeparturedate=" + destinationsRequest.Earliestdeparturedate);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Latestdeparturedate))
            {
                url.Append("&latestdeparturedate=" + destinationsRequest.Latestdeparturedate);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Lengthofstay))
            {
                url.Append("&lengthofstay=" + destinationsRequest.Lengthofstay);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Location))
            {
                url.Append("&location=" + destinationsRequest.Location);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Maxfare))
            {
                url.Append("&maxfare=" + destinationsRequest.Maxfare);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Minfare))
            {
                url.Append("&minfare=" + destinationsRequest.Minfare);
            }

            if (!string.IsNullOrWhiteSpace(destinationsRequest.PointOfSaleCountry))
            {
                url.Append("&pointofsalecountry=" + destinationsRequest.PointOfSaleCountry);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Region))
            {
                url.Append("&region=" + destinationsRequest.Region);
            }

            if (!string.IsNullOrWhiteSpace(destinationsRequest.Theme))
            {
                url.Append("&theme=" + destinationsRequest.Theme);
            }
            if (!string.IsNullOrWhiteSpace(destinationsRequest.TopDestinations))
            {
                url.Append("&topdestinations=" + destinationsRequest.TopDestinations);
            }
            return url.ToString();
        }

        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url, int count = 0)
        {
            //TrippismNLog.SaveNLogData(url);
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            string posResponse = "Parameter 'pointofsalecountry' has an unsupported value";
            if (result.StatusCode == HttpStatusCode.BadRequest && result.Response.ToString() == posResponse)
            {
                APIResponse supportedPOSCountries = GetAPIResponse(SabreCountriesUrl);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, supportedPOSCountries.Response);
                return response;
            }
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_DestinationFinder cities = new OTA_DestinationFinder();
                cities = ServiceStackSerializer.DeSerialize<OTA_DestinationFinder>(result.Response);
                Fares fares = Mapper.Map<OTA_DestinationFinder, Fares>(cities);
                if (count != 0)
                {
                    fares.FareInfo = fares.FareInfo.OrderBy(f => f.LowestFare.Fare).Take(count).ToList();
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fares);
                //TrippismNLog.SaveNLogData(result.Response);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
        private APIResponse GetAPIResponse(string url)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            return result;
        }
        

    }
}