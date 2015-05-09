using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

       
    }
}
