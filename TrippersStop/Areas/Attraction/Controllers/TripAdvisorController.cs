using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraveLayer.CustomTypes.TripAdvisor.Response;
using Trippism.Areas.TripAdvisor.Models;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.Attraction.Controllers
{
    public class TripAdvisorController : Controller
    {
        // GET: Attraction/TripAdvisor
        public ActionResult Index()
        {
            List<SelectListItem> items = GetTypes();
            ViewData["Types"] = items;
            return View();
        }
        [HttpPost]

        public ActionResult Index(AttractionsRequest model, string returnUrl)
        {

            //List<SelectListItem> items = GetTypes();

            //ViewData["Types"] = items;
            TripAdvisorAPICaller ta=new TripAdvisorAPICaller( );
            string url = GetAttractionsApiURL(model);
            var result=ta.Get(url).Result;

            var attractions = ServiceStackSerializer.DeSerialize<LocationInfo>(result.Response);


            if (ModelState.IsValid)
            {

                return View("Display", attractions.data);

            }

            return View(model);

        }

        private string GetAttractionsApiURL(AttractionsRequest attractionsRequest)
        {
            string location = string.Join(",", attractionsRequest.Latitude, attractionsRequest.Longitude);
            System.Text.StringBuilder apiUrl = new System.Text.StringBuilder(string.Format("api/partner/2.0/map/{0}/attractions", location));
            apiUrl.Append("?key={0}");
            if (!string.IsNullOrWhiteSpace(attractionsRequest.Locale))
                apiUrl.Append("&lang=" + attractionsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(attractionsRequest.Currency))
                apiUrl.Append("&lang=" + attractionsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(attractionsRequest.LengthUnit))
                apiUrl.Append("&lang=" + attractionsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(attractionsRequest.Distance))
                apiUrl.Append("&lang=" + attractionsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(attractionsRequest.SubCategory))
                apiUrl.Append("&subcategory=" + attractionsRequest.SubCategory);
            return apiUrl.ToString();
        }
        private List<SelectListItem> GetTypes()
        {

            List<SelectListItem> items = new List<SelectListItem>();



            items.Add(new SelectListItem { Text = "Action", Value = "0" });



            items.Add(new SelectListItem { Text = "Drama", Value = "1" });



            items.Add(new SelectListItem { Text = "Comedy", Value = "2", Selected = true });



            items.Add(new SelectListItem { Text = "Science Fiction", Value = "3" });



            return items;

        }

        public ActionResult LocationInfo()
        {
            return View();
        }
        public ActionResult Display(List<Datum> model)
        {
            return View(model);
        }
    }
}