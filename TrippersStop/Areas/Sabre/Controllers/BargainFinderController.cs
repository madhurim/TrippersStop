using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippersStop.TraveLayer;
using TraveLayer.APIServices;
using ServiceStack;
using AutoMapper;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using System.Xml;
using System.Xml.Linq;


namespace TrippersStop.Areas.Sabre.Controllers
{
    public class BargainFinderController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public BargainFinderController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;          
        }
        // POST api/bargainfinder
        public HttpResponseMessage Post(BargainFinder bargainFinder)
        {
            //BargainFinder bFinder = new BargainFinder();
            ////bFinder.
            //POS pos = new POS();
            //XElement xelement = XElement.Load(System.Web.Hosting.HostingEnvironment.MapPath("/POS/PointOfSales.xml"));
            //var element = from nm in xelement.Elements("POS")
            //           where (string)nm.Element("IsActive") == "true" && (string)nm.Element("RequestType") == "BargainFinder"
            //           select nm;
            ////XmlDocument xmlDocument = new XmlDocument();
            ////xmlDocument.Load(@"\POS\PointOfSales.xml");
            ////XmlNodeList nodeList = xmlDocument.DocumentElement.SelectNodes("/PointOfSales");
            ////var node =nodeList
            ////string proID = "", proName = "", price = "";
            ////foreach (XmlNode node in nodeList)
            ////{
            ////    proID = node.SelectSingleNode("Product_id").InnerText;
            ////    proName = node.SelectSingleNode("Product_name").InnerText;
            ////    price = node.SelectSingleNode("Product_price").InnerText;
            ////}
            //var element1 = from p in xelement.Elements("PointOfSales")
            //               where (string)p.Element("POS").Attribute("IsActive") == "true" && (string)p.Element("POS").Attribute("RequestType") == "BargainFinder"
            //               select p;
            //var e = element1.ElementAt(1).Element("CompanyCode").Value;
            //var f = element1.ElementAt(1).Element("ID").Value;
            //var g = element1.ElementAt(1).Element("Type").Value;

            //TBD : URL configurable using XML
            APIHelper.SetApiKey(_apiCaller, _cacheService);
            String result = _apiCaller.Post("v1.8.2/shop/flights?mode=live", ServiceStackSerializer.Serialize(bargainFinder)).Result;
            var bargainResponse= DeSerializeResponse(result);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, bargainResponse);         
            return response;
        }

        private LowFareSearch DeSerializeResponse(string result)
        {
            BargainFinderReponse reponse = new BargainFinderReponse();
            reponse = ServiceStackSerializer.DeSerialize<BargainFinderReponse>(result);
            Mapper.CreateMap<BargainFinderReponse, LowFareSearch>()
                    .ForMember(o => o.AirLowFareSearchRS, m => m.MapFrom(s => s.OTA_AirLowFareSearchRS));
            LowFareSearch lowFareSearch = Mapper.Map<BargainFinderReponse, LowFareSearch>(reponse);
            return lowFareSearch;
        }

    }

}
