using AutoMapper;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippismApi.TraveLayer;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Web.Http.Description;
using Trippism.APIExtention.Filters;
using System.Configuration;
using System.Threading.Tasks;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// API rates weekly traffic volumes to certain destination airports. The API looks up the traffic volume booked 
    /// </summary>
     [GZipCompressionFilter]
    public class SeasonalityController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public string SabreSeasonalityUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSeasonalityUrl"];
            }
        }
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public SeasonalityController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;       
        }
        /// <summary>
        /// API rates weekly traffic volumes to certain destination airports. The API looks up the traffic volume booked 
        /// </summary>
        [ResponseType(typeof(VM.TravelSeasonality))]
        public async Task<HttpResponseMessage> Get(string destination)
        {
            string url = string.Format(SabreSeasonalityUrl, destination);
            return await Task.Run(() => 
               { return GetResponse(url); }); 

        }
        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url)
        {

            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_TravelSeasonality seasonality = new OTA_TravelSeasonality();
                seasonality = ServiceStackSerializer.DeSerialize<OTA_TravelSeasonality>(result.Response);
                Mapper.CreateMap<OTA_TravelSeasonality, VM.TravelSeasonality>();
                VM.TravelSeasonality travelSeasonality = Mapper.Map<OTA_TravelSeasonality, VM.TravelSeasonality>(seasonality);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, travelSeasonality);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
