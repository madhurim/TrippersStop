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
    public class MapController : ApiController
    {
        const string TrippismKey = "Trippism.TripAdvisor.Map.";
        readonly ITripAdvisorAPIAsyncCaller _apiCaller;
        readonly ICacheService _cacheService;

        /// <summary>
        /// Set Api caller and Cache service
        /// </summary>
        public MapController(ITripAdvisorAPIAsyncCaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }
        private string MapUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorMapUrl"];
            }
        }

        private string MapAttractions
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorMapAttractionsUrl"];
            }
        }


        /// <summary>
        /// The response provides location data for all location
        /// The desired language locale
        /// Currency code to use for request and response 
        /// Length unit (either 'mi' or 'km') which overrides the default length units used in input and output
        /// Distance in miles (unless another unit is specified using lunit) defining thewidth and height of the bounding rectangle around the center when specified. Defaults to 10 miles. The maximum is 25 miles or 50 kilometers.
        /// <param name="hotelMapRequest">
        /// </param>
        /// </summary>
        //[ResponseType(typeof(TripWeather))]
        [Route("api/map")]
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri]MapRequest mapRequest)
        {
            return await Task.Run(() =>
            { return GetMapLocations(mapRequest); });
        }

        /// <summary>
        /// The response provides location data for all location
        /// <param name="hotelMapRequest">
        /// </param>
        /// </summary>
        //[ResponseType(typeof(TripWeather))]
        [Route("api/map/attractions")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAttractions([FromUri]MapRequest mapRequest)
        {
            return await Task.Run(() =>
            { return GetMapAttractions(mapRequest); });
        }

        private IHttpActionResult GetMapAttractions(MapRequest mapRequest)
        {
            throw new NotImplementedException();
        }


        private IHttpActionResult GetMapLocations(MapRequest hotelMapRequest)
        {
            string urlAPI = GetApiURL(hotelMapRequest);
            return Ok("TBD");
        }

        private string GetApiURL(MapRequest hotelMapRequest)
        {
            if (!string.IsNullOrWhiteSpace(hotelMapRequest.Latitude) && !string.IsNullOrWhiteSpace(hotelMapRequest.Longitude))
                
            {
                string location = string.Join(",",hotelMapRequest.Latitude, hotelMapRequest.Longitude);
                string url = string.Format(MapUrl, location);       
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
