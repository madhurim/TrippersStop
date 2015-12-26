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
    public class HotelController : ApiController
    {
        const string TrippismKey = "Trippism.TripAdvisor.Hotel.";
        readonly ITripAdvisorAPIAsyncCaller _apiCaller;
        readonly ICacheService _cacheService;

        /// <summary>
        /// Set Api caller and Cache service
        /// </summary>
        public HotelController(ITripAdvisorAPIAsyncCaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }

        private string HotelMapUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorHotelUrl"];
            }
        }

        /// <summary>
        /// The response provides Hotel data for all location
        /// The desired language locale
        /// Currency code to use for request and response 
        /// Length unit (either 'mi' or 'km') which overrides the default length units used in input and output
        /// Distance in miles (unless another unit is specified using lunit) defining thewidth and height of the bounding rectangle around the center when specified. Defaults to 10 miles. The maximum is 25 miles or 50 kilometers.
        /// <param name="hotelMapRequest">
        /// </param>
        /// </summary>
        //[ResponseType(typeof(TripWeather))]
        [Route("api/tripadvisor/hotel")]
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri]HotelMapRequest hotelMapRequest)
        {
            return await Task.Run(() =>
            { return GetHotel(hotelMapRequest); });
        }
        private IHttpActionResult GetHotel(HotelMapRequest hotelMapRequest)
        {
            string urlAPI = GetApiURL(hotelMapRequest);
            return Ok("TBD");
        }

        private string GetApiURL(HotelMapRequest hotelMapRequest)
        {
            if (!string.IsNullOrWhiteSpace(hotelMapRequest.Latitude) && !string.IsNullOrWhiteSpace(hotelMapRequest.Longitude))
            {
                string location = string.Join(",", hotelMapRequest.Latitude, hotelMapRequest.Longitude);
                string url = string.Format(HotelMapUrl, location);
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
