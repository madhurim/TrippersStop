﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippersStop.TraveLayer;
using TraveLayer.CustomTypes.Sabre.Response;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class SeasonalRateController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public SeasonalRateController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;       
        }
        public HttpResponseMessage Get(string destination)
        {
            string url = string.Format("v1/historical/flights/{0}/seasonality", destination);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {

            APIHelper.SetApiKey(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                _cacheService.Expire(_apiCaller.SabreTokenKey);
                _cacheService.Expire(_apiCaller.SabreTokenExpireKey);
                APIHelper.SetApiKey(_apiCaller, _cacheService);
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
