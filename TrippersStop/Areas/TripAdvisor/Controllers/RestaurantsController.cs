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
        /// Set Api caller and Cache service
        /// </summary>
        public async Task<IHttpActionResult> Get([FromUri]RestaurantMapRequest restaurantMapRequest)
        {
            return await Task.Run(() =>
            { return GetRestaurants(restaurantMapRequest); });
        }

        private IHttpActionResult GetRestaurants(RestaurantMapRequest restaurantMapRequest)
        {
            throw new NotImplementedException();
        }

    }
}
