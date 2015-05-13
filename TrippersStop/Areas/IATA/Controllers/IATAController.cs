using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using DataLayer;
using System.Net.Http;
using System.Reflection;
using System.Net;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.IATA.ViewModels;
using System.Text;
using MongoDB.Driver;
using Trippism.Areas.IATA.Models;


namespace Trippism.Areas.IATA.Controllers
{
    public class IATAController : ApiController
    {
        IDBService _dbService;

        public IATAController(IDBService dbService)
        {
            _dbService = dbService;
        }

        // GET api/DestinationFinder
        public HttpResponseMessage Get()
        {
            return GetResponse();
        }

        private HttpResponseMessage GetResponse()
        {
            List<IATACode> AirPortCodes;
            MongoDatabase database = MongoDBProperty.MongoDB;

            var collection = _dbService.Get<IATACode>("IATA");
            AirPortCodes = collection.ToList();
          
            return Request.CreateResponse(HttpStatusCode.OK, AirPortCodes);
        }
       
     

       
    }
}
