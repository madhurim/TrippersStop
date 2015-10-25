using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippismApi.TraveLayer;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using AutoMapper;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Web.Http.Description;
using Trippism.APIExtention.Filters;
using System.Configuration;
using System.Threading.Tasks;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// API returns the median, highest, and lowest published fares 
    /// </summary>
    [GZipCompressionFilter]
    public class FareRangeController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public string SabreFareRangeUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreFareRangeUrl"];
            }
        }
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public FareRangeController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;         
        }
        // GET api/lowfareforecast
        /// <summary>
        /// API returns the median, highest, and lowest published fares 
        /// </summary>
        [ResponseType(typeof(VM.FareRange))]
        public async Task<HttpResponseMessage> Get([FromUri]VM.FareForecast fareForecastRequest)
        {
            string url = string.Format(SabreFareRangeUrl+"?origin={0}&destination={1}&earliestdeparturedate={2}&latestdeparturedate={3}&lengthofstay={4}", fareForecastRequest.Origin, fareForecastRequest.Destination, fareForecastRequest.EarliestDepartureDate, fareForecastRequest.LatestDepartureDate, fareForecastRequest.LengthOfStay);
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
                OTA_FareRange fares = new OTA_FareRange();
                fares = ServiceStackSerializer.DeSerialize<OTA_FareRange>(result.Response);
                Mapper.CreateMap<OTA_FareRange, VM.FareRange>();
                VM.FareRange fareRange = Mapper.Map<OTA_FareRange, VM.FareRange>(fares);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fareRange);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
