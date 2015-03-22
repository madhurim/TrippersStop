﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippersStop.TraveLayer;
using TrippersStop.Helper;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Sabre;
using AutoMapper;
using TraveLayer.APIServices;


namespace TrippersStop.Areas.Sabre.Controllers
{
    public class LowFareForecastController : ApiController
    {
        // GET api/lowfareforecast
        public HttpResponseMessage Get([FromUri]TravelInfo lowFareForecastRequest)
        {
            string url = string.Format("v1/forecast/flights/fares?origin={0}&destination={1}&departuredate={2}&returndate={3}", lowFareForecastRequest.Origin, lowFareForecastRequest.Destination, lowFareForecastRequest.DepartureDate, lowFareForecastRequest.ReturnDate);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            OTA_LowFareForecast fares = new OTA_LowFareForecast();
            fares = ServiceStackSerializer.DeSerialize<OTA_LowFareForecast>(result);
            Mapper.CreateMap<OTA_LowFareForecast, LowFareForecast>();
            LowFareForecast lowFareForecast = Mapper.Map<OTA_LowFareForecast, LowFareForecast>(fares);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lowFareForecast);
            return response;
        }
    }
}
