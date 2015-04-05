using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippismApi.TraveLayer;
using AutoMapper;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Web.Http.Description;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    ///   Travelers who want to choose a destination based on popularity.
    ///   Travelers who want to choose a destination based on theme, e.g., BEACH .
    ///   Travelers who want to choose a destination based on country or region, e.g., EUROPE .
    /// </summary>
    public class TopDestinationsController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public TopDestinationsController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;            
        }
        // GET api/lookup
        /// <summary>
        /// API retrieves top booked leisure destinations and returns them in ascending rank order.
        /// </summary>
        [HttpGet]
        [ResponseType(typeof(TopDestination))]
        public HttpResponseMessage Get()
        {
            string url = string.Format("v1/lists/top/destinations");
            return GetResponse(url);
        }
        /// <summary>
        /// API retrieves top booked leisure destinations from a given origin, theme, region and returns them in ascending rank order.
        /// </summary>
        [ResponseType(typeof(TopDestination))]
        public HttpResponseMessage GetTopDestinationByTheme(string origin, string theme, string region)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}&theme={1}&region={2}", origin, theme, region);
            return GetResponse(url);
        }
        /// <summary>
        /// API retrieves top booked leisure destinations from a given origin and returns them in ascending rank order.
        /// </summary>
        [ResponseType(typeof(TopDestination))]
        public HttpResponseMessage GetTopDestinationByBackweeks(string origin, string lookbackweeks, string topdestinations)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}&lookbackweeks={1}&topdestinations={2}", origin, lookbackweeks, topdestinations);
            return GetResponse(url);
        }
        /// <summary>
        /// API retrieves top booked leisure destinations from a given airport code and returns them in ascending rank order.
        /// </summary>
        [ResponseType(typeof(TopDestination))]
        public HttpResponseMessage GetTopDestinationByairportCode(string airportCode)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}", airportCode);
            return GetResponse(url);
        }
        /// <summary>
        /// API retrieves top booked leisure destinations from a given origin country and returns them in ascending rank order.
        /// </summary>
        [ResponseType(typeof(TopDestination))]
        public HttpResponseMessage GetTopDestinationByCountryCode(string countryCode)
        {
            string url = string.Format("v1/lists/top/destinations?origincountry={0}", countryCode);
            return GetResponse(url);
        }
        /// <summary>
        /// API retrieves top booked leisure destinations from a given origin, destination type and returns them in ascending rank order.
        /// </summary>
        [ResponseType(typeof(TopDestination))]
        public HttpResponseMessage GetTopDestinationByDestinationType(string origin, string destinationType)
        {
            string url = string.Format("v1/lists/top/destinations?origin={0}&destinationtype={1}", origin,destinationType);
            return GetResponse(url);
        }
        /// <summary>
        /// API retrieves top n booked leisure destinations and returns them in ascending rank order.
        /// </summary>
        [ResponseType(typeof(TopDestination))]
        public HttpResponseMessage GetTopDestinations(int number)
        {
            string url = string.Format("v1/lists/top/destinations?topdestinations={0}", number);
            return GetResponse(url);
        }
        /// <summary>
        /// API retrieves top booked leisure destinations from a given destination country and returns them in ascending rank order.
        /// </summary>
        [ResponseType(typeof(TopDestination))]
        public HttpResponseMessage GetTopDestinationsByDestinationCountry(string destinationcountry)
        {
            string url = string.Format("v1/lists/top/destinations?destinationcountry={0}", destinationcountry);
            return GetResponse(url);
        }
        /// <summary>
        /// API retrieves top booked leisure destinations from a given region and returns them in ascending rank order.
        /// </summary>
        [ResponseType(typeof(TopDestination))]
        public HttpResponseMessage GetTopDestinationsByRegion(string region)
        {
            string url = string.Format("v1/lists/top/destinations?region={0}", region);
            return GetResponse(url);
        }
        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url)
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
                TopDestinations destinations = new TopDestinations();
                destinations = ServiceStackSerializer.DeSerialize<TopDestinations>(result.Response);
                Mapper.CreateMap<TopDestinations, TopDestination>();
                TopDestination topDestinations = Mapper.Map<TopDestinations, TopDestination>(destinations);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, topDestinations);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
