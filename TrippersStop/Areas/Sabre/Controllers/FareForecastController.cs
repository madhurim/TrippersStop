using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippismApi.TraveLayer;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Sabre;
using AutoMapper;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Web.Http.Description;
using Trippism.APIExtention.Filters;
using System.Configuration;


namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// Forecasts the price range into which the lowest published fare that is available 
    /// </summary>
     [GZipCompressionFilter]
    public class FareForecastController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public string SabreFareForecastUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreFareForecastUrl"];
            }
        }
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public FareForecastController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }
        /// <summary>
        /// Forecasts the price range into which the lowest published fare that is available 
        /// </summary>
        // GET api/lowfareforecast
        [ResponseType(typeof(LowFareForecast))]
        public HttpResponseMessage Get([FromUri]TravelInfo lowFareForecastRequest)
        {
            string url = string.Format(SabreFareForecastUrl+"?origin={0}&destination={1}&departuredate={2}&returndate={3}", lowFareForecastRequest.Origin, lowFareForecastRequest.Destination, lowFareForecastRequest.DepartureDate, lowFareForecastRequest.ReturnDate);
            return GetResponse(url);
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
                OTA_LowFareForecast fares = new OTA_LowFareForecast();
                fares = ServiceStackSerializer.DeSerialize<OTA_LowFareForecast>(result.Response);
                Mapper.CreateMap<OTA_LowFareForecast, LowFareForecast>();
                LowFareForecast lowFareForecast = Mapper.Map<OTA_LowFareForecast, LowFareForecast>(fares);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lowFareForecast);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
