using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippismApi.TraveLayer;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Google.Request;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Google.Response;
using ExpressMapper;

namespace Trippism.Areas.Google.Controllers
{
    public class GooglePlaceDetailsController : ApiController
    {
        const string TrippismKey = "Trippism.GooglePlaceDetail.";
        IAsyncGooglePlaceAPICaller _apiCaller;
        ICacheService _cacheService;

        public GooglePlaceDetailsController(IAsyncGooglePlaceAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }


        [ResponseType(typeof(TraveLayer.CustomTypes.Google.ViewModels.GooglePlaceDetails))]
        [Route("api/googleplace/placedetails")]
        [HttpGet]
        public HttpResponseMessage Get([FromUri]GooglePlaceInput locationsearch)
        {
            string cacheKey = TrippismKey + string.Join(".", locationsearch.PlaceId);
            var tripGooglePlace = _cacheService.GetByKey<TraveLayer.CustomTypes.Google.ViewModels.GooglePlaceDetails>(cacheKey);
            if (tripGooglePlace != null && tripGooglePlace.result.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tripGooglePlace);
            }
            return GetResponse(locationsearch.PlaceId, cacheKey);
        }

        private HttpResponseMessage GetResponse(string PlaceId, string cacheKey)
        {
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";

            APIResponse result = _apiCaller.Get(PlaceId).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                GooglePlaceDetailsOutput googleplace = new GooglePlaceDetailsOutput();
                googleplace = ServiceStackSerializer.DeSerialize<GooglePlaceDetailsOutput>(result.Response);

                TraveLayer.CustomTypes.Google.ViewModels.GooglePlaceDetails lstLocations = Mapper.Map<GooglePlaceDetailsOutput, TraveLayer.CustomTypes.Google.ViewModels.GooglePlaceDetails>(googleplace);

                _cacheService.Save<TraveLayer.CustomTypes.Google.ViewModels.GooglePlaceDetails>(cacheKey, lstLocations);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lstLocations);

                return response;
            }

            return Request.CreateResponse(result.StatusCode, result.Response);
        }


    }
}
