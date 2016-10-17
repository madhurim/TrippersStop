using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TrippismEntities;
using TrippismProfiles.Constants;
using TrippismProfiles.Models;
using TrippismRepositories;

namespace TrippismProfiles.Controllers
{
    /// <summary>
    /// It's used to track the Anonymous Customer.
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class AnonymousController : ApiController
    {
        IAnonymousRepository _IAnonymousRepository;


                /// <summary>
        /// Set api - Anonymous Repository.
        /// </summary>
        public AnonymousController(IAnonymousRepository iAnonymousRepository)
        {
            _IAnonymousRepository = iAnonymousRepository;
        }
        /// <summary>
        /// It's used to get the Anonymous Customer based on anonymousId.
        /// </summary>
        [Route("api/profiles/anonymous")]
        public async Task<HttpResponseMessage> Get(Guid anonymousId)
        {
            return await Task.Run(() =>
            { return GetUser(anonymousId); });
        }
        /// <summary>
        /// Create new Anonymous Customer.
        /// </summary>
        [Route("api/profiles/anonymous")]
        public async Task<HttpResponseMessage> Post(Anonymous anonoymous)
        {
            return await Task.Run(() =>
            { return SaveUser(anonoymous); });
        }
        /// <summary>
        /// Update existing Anonymous Customer.
        /// </summary>
        [Route("api/profiles/anonymous")]
    /*    public async Task<HttpResponseMessage> Put(Anonymous anonymousUser)
        {
            return await Task.Run(() =>
            { return UpdateUser(anonymousUser); });
        }

        private HttpResponseMessage UpdateUser(AnonymousViewModel anonymousUser)
        {
            var user = _IAnonymousRepository.GetCustomer(anonymousUser.Id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, TrippismConstants.CustomerNotFound);
            }
            //MM: this is not needed perhaps
            //user.KnownGuid = anonymousUser.KnownGuid;
            _IAnonymousRepository.UpdateCustomer(anonymousUser);
            return Request.CreateResponse(HttpStatusCode.OK);
        }*/

        private HttpResponseMessage SaveUser(Anonymous anonoymous)
        {
            HttpRequestBase baseRequest = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request as HttpRequestBase;
            
            HttpBrowserCapabilitiesBase browser = baseRequest.Browser;
          
            Anonymous newanonym = new Anonymous();
            newanonym.VisitedTime = DateTime.Now;            
            newanonym.VisitorGuid = Guid.NewGuid();
            newanonym.Browser = browser.Browser;
            newanonym.Device = browser.Platform;
            newanonym.City = anonoymous.City;
            newanonym.Region = anonoymous.Region;
            newanonym.Country = anonoymous.Country;
            newanonym.Ipaddress = anonoymous.Ipaddress;

            _IAnonymousRepository.AddCustomer(newanonym);
            return Request.CreateResponse(HttpStatusCode.OK, newanonym.VisitorGuid);
        }

        private HttpResponseMessage GetUser(Guid anonymousId)
        {
            var anonymousUser = _IAnonymousRepository.GetCustomer(anonymousId);
            return Request.CreateResponse(HttpStatusCode.OK, anonymousUser);
        }
    }
}
