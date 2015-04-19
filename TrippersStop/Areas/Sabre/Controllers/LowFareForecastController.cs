﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippersStop.TraveLayer;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Sabre;
using AutoMapper;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre.Response;


namespace TrippersStop.Areas.Sabre.Controllers
{
    public class LowFareForecastController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public LowFareForecastController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }
        // GET api/lowfareforecast
        public HttpResponseMessage Get([FromUri]TravelInfo lowFareForecastRequest)
        {
            string url = string.Format("v1/forecast/flights/fares?origin={0}&destination={1}&departuredate={2}&returndate={3}", lowFareForecastRequest.Origin, lowFareForecastRequest.Destination, lowFareForecastRequest.DepartureDate, lowFareForecastRequest.ReturnDate);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            APIHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                APIHelper.RefreshApiToken(_cacheService, _apiCaller);
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
