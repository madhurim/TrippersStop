using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Trippism.Areas.TripAdvisor.Models;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.TripAdvisor.Controllers
{
    /// <summary>
    /// When specifying a Lat/Long point, returns a list of 10 properties found within a given distance from that point. If there are more than 10 properties within the radius requested, the 10 nearest properties will be returned.   In lieu of lat long, can specify a location ID and the output will return nearest POIs
    /// </summary>
    public class AttractionsController : ApiController
    {
        const string TrippismKey = "Trippism.TripAdvisor.Attractions.";
        readonly ITripAdvisorAPIAsyncCaller _apiCaller;
        readonly ICacheService _cacheService;

        /// <summary>
        /// Set Api caller and Cache service
        /// </summary>
        public AttractionsController(ITripAdvisorAPIAsyncCaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }
        private string APIAllAttractionsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorAllAttractionsUrl"];
            }
        }

        private string APITopAttractions
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorTopAttractionsUrl"];
            }
        }


        /// <summary>
        /// The response provides all available attractions
        /// </summary>
        //[ResponseType(typeof(TripWeather))]
        [Route("api/tripadvisor/attractions/all")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll([FromUri]MapRequest mapRequest)
        {
            return await Task.Run(() =>
            { return GetAllAttractions(mapRequest); });
        }

        /// <summary>
        /// The response provides top 10 attractions
        /// </summary>
        //[ResponseType(typeof(TripWeather))]
        [Route("api/tripadvisor/attractions")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTopAttractions([FromUri]MapRequest mapRequest)
        {
            return await Task.Run(() =>
            { return GetTopMapAttractions(mapRequest); });
        }

        private IHttpActionResult GetTopMapAttractions(MapRequest mapRequest)
        {
            throw new NotImplementedException();
        }


        private IHttpActionResult GetAllAttractions(MapRequest hotelMapRequest)
        {
            string urlAPI = GetApiURL(hotelMapRequest);
            return Ok("TBD");
        }

        private string GetApiURL(MapRequest hotelMapRequest)
        {
            if (!string.IsNullOrWhiteSpace(hotelMapRequest.Latitude) && !string.IsNullOrWhiteSpace(hotelMapRequest.Longitude))
                
            {
                string location = string.Join(",",hotelMapRequest.Latitude, hotelMapRequest.Longitude);
                string url = string.Format(APIAllAttractionsUrl, location);       
                //    if (!string.IsNullOrWhiteSpace(hotelMapRequest.Locale))
                //        url += string.Format("&pagetoken={0}", hotelMapRequest.Locale);
           
                //    if (!string.IsNullOrWhiteSpace(hotelMapRequest.Distance))
                //        url += hotelMapRequest.Distance;
                //    if (!string.IsNullOrWhiteSpace(hotelMapRequest.LengthUnit))
                //        url += hotelMapRequest.LengthUnit;
                    return url;
            }
            return string.Empty;
         
        }
    }
}
