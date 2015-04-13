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
        IAsyncSabreAPICaller apiCaller;
        public BargainFinderController(IAsyncSabreAPICaller repository, ICacheService cacheService)
        {
            apiCaller = repository;
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            apiCaller.LongTermToken = cacheService.GetByKey<string>(apiCaller.SabreTokenKey);
            apiCaller.TokenExpireIn = cacheService.GetByKey<string>(apiCaller.SabreTokenExpireKey);
            if (string.IsNullOrWhiteSpace(apiCaller.LongTermToken))
            {
                apiCaller.LongTermToken = apiCaller.GetToken().Result;
            }
            double expireTimeInSec;
            if (!string.IsNullOrWhiteSpace(apiCaller.TokenExpireIn) && double.TryParse(apiCaller.TokenExpireIn, out expireTimeInSec))
            {
                cacheService.Save<string>(apiCaller.SabreTokenKey, apiCaller.LongTermToken, expireTimeInSec / 60);
                cacheService.Save<string>(apiCaller.SabreTokenExpireKey, apiCaller.TokenExpireIn, expireTimeInSec / 60);
            }

            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
        // POST api/bargainfinder
        public HttpResponseMessage Post(BargainFinder bargainFinder)
        {
            BargainFinder bFinder = new BargainFinder();
            //bFinder.
            POS pos = new POS();
            XElement xelement = XElement.Load(System.Web.Hosting.HostingEnvironment.MapPath("/POS/PointOfSales.xml"));
            var element = from nm in xelement.Elements("POS")
                       where (string)nm.Element("IsActive") == "true" && (string)nm.Element("RequestType") == "BargainFinder"
                       select nm;
            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.Load(@"\POS\PointOfSales.xml");
            //XmlNodeList nodeList = xmlDocument.DocumentElement.SelectNodes("/PointOfSales");
            //var node =nodeList
            //string proID = "", proName = "", price = "";
            //foreach (XmlNode node in nodeList)
            //{
            //    proID = node.SelectSingleNode("Product_id").InnerText;
            //    proName = node.SelectSingleNode("Product_name").InnerText;
            //    price = node.SelectSingleNode("Product_price").InnerText;
            //}
            var element1 = from p in xelement.Elements("PointOfSales")
                           where (string)p.Element("POS").Attribute("IsActive") == "true" && (string)p.Element("POS").Attribute("RequestType") == "BargainFinder"
                           select p;
            var e = element1.ElementAt(1).Element("CompanyCode").Value;
            var f = element1.ElementAt(1).Element("ID").Value;
            var g = element1.ElementAt(1).Element("Type").Value;

            SabreAPICaller bargainFinderAPI = new SabreAPICaller();
            bargainFinderAPI.Accept = "application/json";
            bargainFinderAPI.ContentType = "application/x-www-form-urlencoded";
            //TBD : Aoid call for getting token
            string token = bargainFinderAPI.GetToken().Result;
            bargainFinderAPI.Authorization = "bearer";
            bargainFinderAPI.ContentType = "application/json";
            //TBD : URL configurable using XML
            String result = bargainFinderAPI.Post("v1.8.2/shop/flights?mode=live", ServiceStackSerializer.Serialize(bargainFinder)).Result;
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
