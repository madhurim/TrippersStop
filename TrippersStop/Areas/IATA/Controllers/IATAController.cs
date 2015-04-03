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



namespace TrippismApi.Areas.IATA.Controllers
{
    /// <summary>
    /// API to retrieve data for city pairs.
    /// </summary>
    public class IATAController : ApiController
    {
        IDBService _dbService;
        /// <summary>
        /// Set db service.
        /// </summary>
        public IATAController(IDBService dbService)
        {
            _dbService = dbService;
        }


        /// <summary>
        /// Get available city pairs.
        /// </summary>
        public HttpResponseMessage Get()
        {
            return GetResponse();
        }
        /// <summary>
        /// Get response from IATA service..
        /// </summary>
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
