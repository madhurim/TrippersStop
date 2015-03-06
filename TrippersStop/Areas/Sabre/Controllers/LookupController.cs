using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class LookupController : ApiController
    {
        // GET api/lookup
        //[HttpGet]
        //public IEnumerable<string> Get([FromUri]TopDestinationRequest topDestinationRequest)
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/lookup/5
        public string Get(string id)
        {
            return "value";
        }

        // POST api/lookup
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/lookup/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/lookup/5
        //public void Delete(int id)
        //{
        //}
    }
}
