using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.TripAdvisor.Response;
using TraveLayer.CustomTypes.TripAdvisor.ViewModels;
using Trippism.APIExtention.Filters;
using Trippism.Areas.TripAdvisor.Models;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.TripAdvisor.Controllers
{
    /// <summary>
    /// When specifying a Lat/Long point, returns a list of 10 properties found within a given distance from that point. If there are more than 10 properties within the radius requested, the 10 nearest properties will be returned.   In lieu of lat long, can specify a location ID and the output will return nearest POIs
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class RestaurantsController : ApiController
    {

        readonly ITripAdvisorAPIAsyncCaller _apiCaller;

        /// <summary>
        /// Set Api caller and Cache service
        /// </summary>
        public RestaurantsController(ITripAdvisorAPIAsyncCaller apiCaller)
        {
            _apiCaller = apiCaller;

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
            string urlAPI = GetApiURL(restaurantMapRequest);
            APIResponse result = _apiCaller.Get(urlAPI).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var restaurants = ServiceStackSerializer.DeSerialize<LocationInfo>(result.Response);
                var locations = Mapper.Map<LocationInfo, LocationAttraction>(restaurants);
                return Ok(locations);
            }
            return ResponseMessage(new HttpResponseMessage(result.StatusCode));
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
