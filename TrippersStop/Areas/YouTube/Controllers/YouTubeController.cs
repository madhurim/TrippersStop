using AutoMapper;
using System.Collections.Generic;
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

                Mapper.CreateMap<YouTubeOutput, TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>();
                Mapper.CreateMap<Items, TraveLayer.CustomTypes.YouTube.ViewModels.Items>();
                Mapper.CreateMap<Snippet, TraveLayer.CustomTypes.YouTube.ViewModels.Snippet>();
                Mapper.CreateMap<Id, TraveLayer.CustomTypes.YouTube.ViewModels.Id>();
                Mapper.CreateMap<Thumbnails, TraveLayer.CustomTypes.YouTube.ViewModels.Thumbnails>();
                Mapper.CreateMap<Default, TraveLayer.CustomTypes.YouTube.ViewModels.Default>();
                Mapper.CreateMap<Medium, TraveLayer.CustomTypes.YouTube.ViewModels.Medium>();
                Mapper.CreateMap<High, TraveLayer.CustomTypes.YouTube.ViewModels.High>();
                Mapper.CreateMap<pageInfo, TraveLayer.CustomTypes.YouTube.ViewModels.pageInfo>();

                TraveLayer.CustomTypes.YouTube.ViewModels.YouTube lstVideos = Mapper.Map<YouTubeOutput, TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>(youtube);

                _cacheService.Save<TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>(cacheKey, lstVideos);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lstVideos);
                
                return response;
            }

            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}