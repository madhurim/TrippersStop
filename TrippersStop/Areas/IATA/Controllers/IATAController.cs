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
        //
        // GET: /IATA/IATA/
        MongoService service = new MongoService();


        // GET api/DestinationFinder
        public HttpResponseMessage Get()
        {
            return GetResponse();
        }

        private HttpResponseMessage GetResponse()
        {
            List<IATACode> AirPortCodes;
            MongoDatabase database = MongoDBProperty.MongoDB;
            
            MongoCollection collection = database.GetCollection<Entity>("IATA");

            var entity = new Entity { Name = "IATA" };
            //collection.Insert(entity);
            //var id = entity.Id;

            var collection1 = service.Get<IATACode>("IATA");// Get11();
            AirPortCodes = collection1.ToList();
          
            return Request.CreateResponse(HttpStatusCode.OK, AirPortCodes);
        }
       
     

       
    }
}
