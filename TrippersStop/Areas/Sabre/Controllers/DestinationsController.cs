using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippismApi.TraveLayer;
using ServiceStack;
using System.Reflection;
using System.Text;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using AutoMapper;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Configuration;


namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// Return the current nonstop lead fare and an overall lead fare available to destinations
    /// </summary>
    public class DestinationsController : ApiController
    {

        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        const string _destinationKey = "TrippismApi.Destinations.All";
        string _expireTime = ConfigurationManager.AppSettings["RedisExpireInMin"].ToString();

        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public DestinationsController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;      
        }

        // GET api/DestinationFinder
        /// <summary>
        /// Return the current nonstop lead fare and an overall lead fare available to destinations from a specific origin on roundtrip travel dates 
        /// </summary>
        /// <param name="destinationsRequest">
        /// Return record based on destinations Request Type</param>
        public HttpResponseMessage Get([FromUri]Destinations destinationsRequest)
        {
            string url = GetURL(destinationsRequest);
            return GetResponse(url);
        }

        // GET api/DestinationFinder
        /// <summary>
        /// Filters the response for destination airport codes associated with a travel theme
        /// </summary>
        [Route("api/destinations/theme/{theme}")]
        [HttpGet]
        public HttpResponseMessage GetDestinationsByTheme(string theme,string origin,string departuredate,string returndate,string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&lengthofstay={3}&theme={4}", origin, departuredate, returndate, lengthofstay, theme);
            return GetResponse(url);
        }
        // GET api/DestinationFinder
        /// <summary>
        /// Return the cheapest detinations 
        /// </summary>
        [Route("api/destinations/cheapest/{count}")]
        [HttpGet]
        public HttpResponseMessage GetTopCheapestDestinations(int count, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&lengthofstay={3}", origin, departuredate, returndate, lengthofstay);
            return GetResponse(url, count);
        }
        // GET api/DestinationFinder
        /// <summary>
        /// Filters the response for overall lead fares that are equal to or less than the maximum, and returns the value in LowestFare
        /// </summary>
        [Route("api/destinations/maxfare/{maxfare}")]
        [HttpGet]
        public HttpResponseMessage GetDestinationsByMaxFare(double maxfare, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&maxfare={3}&lengthofstay={4}", origin, departuredate, returndate,maxfare, lengthofstay);
            return GetResponse(url);
        }
        // GET api/DestinationFinder
        /// <summary>
        /// Filters the response for destinations in the country or countries you specify
        /// </summary>
        [Route("api/destinations/country/{country}")]
        [HttpGet]
        public HttpResponseMessage GetDestinationsByCountry(string country, string origin, string departuredate, string returndate, string lengthofstay)
        {
            string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&maxfare={3}&lengthofstay={4}&location={4}", origin, departuredate, returndate, lengthofstay, country);
            return GetResponse(url);
        }

        /// <summary>
        /// Format url based on request.
        /// </summary>
        private string GetURL(Destinations destinationsRequest)
        {
            StringBuilder url = new StringBuilder();
            url.Append("v1/shop/flights/fares?");
            //url.Append("v1/shop/flights?");
            Type type = destinationsRequest.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string propertyName = string.Empty;
            string propertyValue = string.Empty;
            string separator = string.Empty;
            foreach (PropertyInfo property in properties)
            {
                propertyName = property.Name;
                var result = property.GetValue(destinationsRequest, null);
                if (result != null)
                {
                    propertyValue = result.ToString();
                    if (!string.IsNullOrWhiteSpace(propertyValue))
                    {
                        url.Append(separator);
                        url.Append(string.Join("=", propertyName.ToLower(), propertyValue));
                        //url.Append(string.Join("=", propertyName, propertyValue));
                        separator = "&";
                    }
                }
            }
            return url.ToString();
        }
        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url, int count = 0)
        {
            SabreApiTokenHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                SabreApiTokenHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_DestinationFinder cities = new OTA_DestinationFinder();
                cities = ServiceStackSerializer.DeSerialize<OTA_DestinationFinder>(result.Response);
                Mapper.CreateMap<OTA_DestinationFinder, Fares>();
                Fares fares = Mapper.Map<OTA_DestinationFinder, Fares>(cities);
                if (count != 0)
                {
                    fares.FareInfo = fares.FareInfo.OrderBy(f => f.LowestFare).Take(count).ToList();
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fares);
                return response;
            }         
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}