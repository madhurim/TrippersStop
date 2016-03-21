using ExpressMapper;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.YouTube.Request;
using TraveLayer.CustomTypes.YouTube.Response;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.YouTube.Controllers
{
    public class YouTubeController : ApiController
    {
        const string TrippismKey = "Trippism.YouTube."; 
        IAsyncYouTubeAPICaller _apiCaller;
        ICacheService _cacheService;

        public YouTubeController(IAsyncYouTubeAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;         
        }

        [ResponseType(typeof(TraveLayer.CustomTypes.YouTube.ViewModels.YouTube))]
        [Route("api/youtube/locationsearch")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri]YouTubeInput locationsearch)
        {
            string cacheKey = TrippismKey + string.Join(".", locationsearch.location);
            var tripYouTube = _cacheService.GetByKey<TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>(cacheKey);
            if (tripYouTube != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tripYouTube);
            }
             return  await Task.Run(() =>
             { return GetResponse(locationsearch.location, cacheKey); }); 
        }

        private HttpResponseMessage GetResponse(string Location, string cacheKey)
        {
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";

            string strLatitudeandsLongitude = Location;

            APIResponse result = _apiCaller.Get(strLatitudeandsLongitude).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                YouTubeOutput youtube = new YouTubeOutput();
                youtube = ServiceStackSerializer.DeSerialize<YouTubeOutput>(result.Response);
                TraveLayer.CustomTypes.YouTube.ViewModels.YouTube lstVideos = Mapper.Map<YouTubeOutput, TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>(youtube);

                _cacheService.Save<TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>(cacheKey, lstVideos);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lstVideos);
                
                return response;
            }

            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}