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
            string url = GetURL();
            return GetResponse();
        }
        private string GetURL()
        {
            StringBuilder url = new StringBuilder();
            url.Append("v1/IATA/Codes?");
            return url.ToString();
        }
        private HttpResponseMessage GetResponse()
        {
            List<IATACode> AirPortCodes;
            MongoDatabase database = MongoDBProperty.MongoDB;
            MongoService service = new MongoService();
            MongoCollection collection = database.GetCollection<Entity>("TestIATA");

            var entity = new Entity { Name = "test" };
            collection.Insert(entity);
            var id = entity.Id;

            var collection1 = service.Get<IATACode>("IATA");// Get11();
            AirPortCodes = collection1.ToList();
          
            return Request.CreateResponse(HttpStatusCode.OK, AirPortCodes);
        }
       
     

       
    }
}
