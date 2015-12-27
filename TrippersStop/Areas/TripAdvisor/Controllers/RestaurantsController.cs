using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Trippism.Areas.TripAdvisor.Models;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.TripAdvisor.Controllers
{
    /// <summary>
    /// When specifying a Lat/Long point, returns a list of 10 properties found within a given distance from that point. If there are more than 10 properties within the radius requested, the 10 nearest properties will be returned.   In lieu of lat long, can specify a location ID and the output will return nearest POIs
    /// </summary>
    public class RestaurantsController : ApiController
    {
        const string TrippismKey = "Trippism.TripAdvisor.Restaurants.";
        readonly ITripAdvisorAPIAsyncCaller _apiCaller;
        readonly ICacheService _cacheService;

        /// <summary>
        /// Set Api caller and Cache service
        /// </summary>
        public RestaurantsController(ITripAdvisorAPIAsyncCaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }
        private string APIRestaurantsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorRestaurantUrl"];
            }
        }

        /// <summary>
        /// The response provides results to a maximum of 10 restaurants  
        /// </summary>
        [Route("api/tripadvisor/restaurants")]
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri]RestaurantsRequest restaurantMapRequest)
        {
            return await Task.Run(() =>
            { return GetRestaurants(restaurantMapRequest); });
        }

        private IHttpActionResult GetRestaurants(RestaurantsRequest restaurantMapRequest)
        {
            throw new NotImplementedException();
        }
        private string GetApiURL(RestaurantsRequest restaurantsRequest)
        {
            string location = string.Join(",", restaurantsRequest.Latitude, restaurantsRequest.Longitude);
            StringBuilder apiUrl = new StringBuilder(string.Format(APIRestaurantsUrl, location));
            apiUrl.Append("?key={0}");
            if (!string.IsNullOrWhiteSpace(restaurantsRequest.Locale))
                apiUrl.Append("&lang=" + restaurantsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(restaurantsRequest.Currency))
                apiUrl.Append("&lang=" + restaurantsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(restaurantsRequest.LengthUnit))
                apiUrl.Append("&lang=" + restaurantsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(restaurantsRequest.Distance))
                apiUrl.Append("&lang=" + restaurantsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(restaurantsRequest.SubCategory))
                apiUrl.Append("&subcategory=" + restaurantsRequest.SubCategory);          
            if (!string.IsNullOrWhiteSpace(restaurantsRequest.Cuisines))
                apiUrl.Append("&cuisines=" + restaurantsRequest.Cuisines);
            if (!string.IsNullOrWhiteSpace(restaurantsRequest.Prices))
                apiUrl.Append("&prices=" + restaurantsRequest.Prices);
           
            return apiUrl.ToString();
        }
    }
}
