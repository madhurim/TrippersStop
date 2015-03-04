using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class DestinationFinderController : ApiController
    {
        // GET api/destinationfinder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/destinationfinder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/destinationfinder
        public void Post([FromBody]string value)
        {
        }

        // PUT api/destinationfinder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/destinationfinder/5
        public void Delete(int id)
        {
        }
    }
}
