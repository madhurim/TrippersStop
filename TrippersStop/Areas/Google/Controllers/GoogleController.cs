using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Google.Request;
using TraveLayer.CustomTypes.Google.Response;
using TraveLayer.CustomTypes.Sabre.Response;
using TrippismApi.TraveLayer;
using AutoMapper;
using System.Threading.Tasks;

namespace Trippism.Areas.GooglePlace.Controllers
{
    public class GooglePlaceController : ApiController
    {
        const string TrippismKey = "Trippism.GooglePlace.";
        IAsyncGoogleAPICaller _apiCaller;
        ICacheService _cacheService;

        public GooglePlaceController(IAsyncGoogleAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }

        [ResponseType(typeof(TraveLayer.CustomTypes.Google.ViewModels.Google))]
        [Route("api/googleplace/locationsearch")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri]GoogleInput locationsearch)
        {
            string cacheKey = TrippismKey + string.Join(".", locationsearch.Latitude, locationsearch.Longitude);
            var tripGooglePlace = _cacheService.GetByKey<TraveLayer.CustomTypes.Google.ViewModels.Google>(cacheKey);
            if (tripGooglePlace != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tripGooglePlace);
            }
             return  await Task.Run(() =>
             { return GetResponse(locationsearch.Latitude, locationsearch.Longitude, cacheKey); }); 
        }

        private HttpResponseMessage GetResponse(string Latitude, string Longitude, string cacheKey)
        {
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";

            string strLatitudeandsLongitude = Latitude + "," + Longitude;

            APIResponse result = _apiCaller.Get(strLatitudeandsLongitude).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                GoogleOutput googleplace = new GoogleOutput();
                googleplace = ServiceStackSerializer.DeSerialize<GoogleOutput>(result.Response);

                Mapper.CreateMap<GoogleOutput, TraveLayer.CustomTypes.Google.ViewModels.Google>();
                Mapper.CreateMap<results, TraveLayer.CustomTypes.Google.ViewModels.results>();
                Mapper.CreateMap<Geometry, TraveLayer.CustomTypes.Google.ViewModels.Geometry>();
                Mapper.CreateMap<Location, TraveLayer.CustomTypes.Google.ViewModels.Location>();
                Mapper.CreateMap<Photos, TraveLayer.CustomTypes.Google.ViewModels.Photos>();

                TraveLayer.CustomTypes.Google.ViewModels.Google lstLocations = Mapper.Map<GoogleOutput, TraveLayer.CustomTypes.Google.ViewModels.Google>(googleplace);

                _cacheService.Save<TraveLayer.CustomTypes.Google.ViewModels.Google>(cacheKey, lstLocations);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lstLocations);

                return response;
            }

            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}