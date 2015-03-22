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
    public class ThemeAirportController : ApiController
    {
        public HttpResponseMessage Get(string theme)
        {
            string url = string.Format("v1/shop/themes/{0}", theme);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            OTA_ThemeAirportLookup themeAirportLookup = new OTA_ThemeAirportLookup();
            themeAirportLookup = ServiceStackSerializer.DeSerialize<OTA_ThemeAirportLookup>(result);
            Mapper.CreateMap<OTA_ThemeAirportLookup, ThemeAirport>();
            ThemeAirport themeAirport = Mapper.Map<OTA_ThemeAirportLookup, ThemeAirport>(themeAirportLookup);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, themeAirport);
            return response;
        }
    }
}
