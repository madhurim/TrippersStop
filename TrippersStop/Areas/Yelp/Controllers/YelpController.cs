using ExpressMapper;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Yelp.Request;
using TraveLayer.CustomTypes.Yelp.Response;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.Yelp.Controllers
{
    public class YelpController : ApiController
    {
        const string TrippismKey = "Trippism.Yelp.";
        IAsyncYelpAPICaller _apiCaller;
        ICacheService _cacheService;

        public YelpController(IAsyncYelpAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;         
        }

        [ResponseType(typeof(TraveLayer.CustomTypes.Yelp.ViewModels.Yelp))]
        [Route("api/yelp/locationsearch")]
        [HttpGet]
        public HttpResponseMessage Get([FromUri]YelpInput locationsearch)
        {
            string cacheKey = TrippismKey + string.Join(".", locationsearch.Latitude, locationsearch.Longitude);
            var tripYelp = _cacheService.GetByKey<TraveLayer.CustomTypes.Yelp.ViewModels.Yelp>(cacheKey);
            if (tripYelp != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tripYelp);
            }
            return GetResponse(locationsearch.Latitude, locationsearch.Longitude, cacheKey);
        }

        private HttpResponseMessage GetResponse(string Latitude, string Longitude, string cacheKey)
        {
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";

            string strLatitudeandsLongitude = Latitude + "," + Longitude;

            APIResponse result = _apiCaller.Get(strLatitudeandsLongitude).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                YelpOutput yelp = new YelpOutput();
                yelp = ServiceStackSerializer.DeSerialize<YelpOutput>(result.Response);
                TraveLayer.CustomTypes.Yelp.ViewModels.Yelp lstLocations = Mapper.Map<YelpOutput, TraveLayer.CustomTypes.Yelp.ViewModels.Yelp>(yelp);
                _cacheService.Save<TraveLayer.CustomTypes.Yelp.ViewModels.Yelp>(cacheKey, lstLocations);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lstLocations);
                return response;
            }

            return Request.CreateResponse(result.StatusCode, result.Response);
        }

    }
}