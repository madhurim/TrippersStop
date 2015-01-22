using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TrippersStop.Areas.Disney
{
    public class DisneyController : ApiController
    {
        // GET api/disney
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/disney/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/disney
        public void Post([FromBody]string value)
        {
        }

        // PUT api/disney/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/disney/5
        public void Delete(int id)
        {
        }
    }
}
