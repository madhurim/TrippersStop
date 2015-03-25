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
using TrippersStop.Helper;
using AutoMapper;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class BargainFinderController : ApiController
    {
        // POST api/bargainfinder
        public HttpResponseMessage Post(BargainFinder bargainFinder)
        {
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
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);         
            return response;
        }

        private BargainFinder DeSerializeResponse(string result)
        {
            BargainFinderReponse reponse = new BargainFinderReponse();
            reponse = ServiceStackSerializer.DeSerialize<BargainFinderReponse>(result);
            Mapper.CreateMap<BargainFinderReponse, BargainFinder>();
            BargainFinder bargainFinder = Mapper.Map<BargainFinderReponse, BargainFinder>(reponse);
            return bargainFinder;
        }

    }

}
