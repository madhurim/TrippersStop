using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippersStop.Helper;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class TravelThemeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string url = string.Format("v1/lists/supported/shop/themes");
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            OTA_TravelThemeLookup travelThemeLookup = new OTA_TravelThemeLookup();
            travelThemeLookup = ServiceStackSerializer.DeSerialize<OTA_TravelThemeLookup>(result);
            Mapper.CreateMap<OTA_TravelThemeLookup, TravelTheme>();
            TravelTheme travelTheme = Mapper.Map<OTA_TravelThemeLookup, TravelTheme>(travelThemeLookup);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, travelTheme);
            return response;
        }
    }
}
