using AutoMapper;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
        public HttpResponseMessage Get([FromUri]YouTubeInput locationsearch)
        {
            string cacheKey = TrippismKey + string.Join(".", locationsearch.location);
            var tripYouTube = _cacheService.GetByKey<TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>(cacheKey);
            if (tripYouTube != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tripYouTube);
            }
            return GetResponse(locationsearch.location, cacheKey);
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

                List<Items> lstItems = youtube.items;

                Mapper.CreateMap<Items, TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>()
                   .ForMember(h => h.VideoId, m => m.MapFrom(s => s.id.videoid))
                   .ForMember(h => h.Title, m => m.MapFrom(s => s.snippet.title))
                   .ForMember(h => h.Description, m => m.MapFrom(s => s.snippet.description))
                   .ForMember(h => h.ChannelId, m => m.MapFrom(s => s.snippet.channelid))
                   .ForMember(h => h.DefaultURL, m => m.MapFrom(s => s.snippet.thumbnails.@default.url))
                   .ForMember(h => h.MediumURL, m => m.MapFrom(s => s.snippet.thumbnails.medium.url))
                   .ForMember(h => h.HighURL, m => m.MapFrom(s => s.snippet.thumbnails.high.url));

                List<TraveLayer.CustomTypes.YouTube.ViewModels.YouTube> lstLocations = Mapper.Map<List<Items>, List<TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>>(lstItems);
                _cacheService.Save<List<TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>>(cacheKey, lstLocations);
                
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lstLocations);
                return response;
            }

            return Request.CreateResponse(result.StatusCode, result.Response);
        }

    }
}