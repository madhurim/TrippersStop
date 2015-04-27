using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
//using System.Web.Http;
using System.Web.Http.Hosting;
//using System.Web.Http;
using System.Web.Mvc;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippersStop.Areas.Sabre.Controllers;
using TrippersStop.TraveLayer;

namespace TrippersStop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Autocomplete(string term)
        {
            var result = new List<KeyValuePair<string, string>>();

            IList<SelectListItem> List = new List<SelectListItem>();
            List.Add(new SelectListItem { Text = "nyc", Value = "0" });
            List.Add(new SelectListItem { Text = "mia", Value = "1" });
            List.Add(new SelectListItem { Text = "bos", Value = "2" });
            List.Add(new SelectListItem { Text = "Lon", Value = "3" });
            List.Add(new SelectListItem { Text = "mia", Value = "4" });
            List.Add(new SelectListItem { Text = "clt", Value = "5" });
            List.Add(new SelectListItem { Text = "bwi", Value = "6" });

            foreach (var item in List)
            {
                result.Add(new KeyValuePair<string, string>(item.Value.ToString(), item.Text));
            }
            var resultDes = result.Where(s => s.Value.ToLower().Contains(term.ToLower())).Select(w => w).ToList();
            return Json(resultDes, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SearchResult(string org,string fdate,string tdate)
        {
            IAsyncSabreAPICaller apiCaller = new SabreAPICaller();
            ICacheService dbService = new RedisService();
            var controller = new DestinationsController(apiCaller, dbService);
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new  System.Web.Http.HttpConfiguration());
            // Act
            Destinations ds = new Destinations();
           
            ds.Origin = org;
            ds.DepartureDate = fdate;// "2015-05-25T00:00:00";//date + "T00:00:00";//
            ds.ReturnDate = tdate;// "2015-05-26T00:00:00";//tdate+"T00:00:00;// 
            ds.Lengthofstay = "4";
            var response = controller.Get(ds);
            var response1 =  response.Content.ReadAsStringAsync().Result;
            
            OTA_DestinationFinder cities = new OTA_DestinationFinder();
            cities = ServiceStackSerializer.DeSerialize<OTA_DestinationFinder>(response1);
            Mapper.CreateMap<OTA_DestinationFinder, Fares>();
            Fares fares = Mapper.Map<OTA_DestinationFinder, Fares>(cities);
            var fRes = fares.FareInfo.Take(10);
            return Json(fRes, JsonRequestBehavior.AllowGet);
        } 
    }
}
