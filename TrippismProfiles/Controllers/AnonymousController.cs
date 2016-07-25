using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public async Task<HttpResponseMessage> Post(AnonymousViewModel anonymousUser)
        {
            return await Task.Run(() =>
            { return SaveUser(anonymousUser); });
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

        private HttpResponseMessage SaveUser(AnonymousViewModel anonymousUser)
        {
          /*  if (anonymousUser == null)
                anonymousUser = new Anonymous(); */
            anonymousUser.VisitedTime = DateTime.Now;
            //anonymousUser.VisitorGuid = Guid.NewGuid();
            Anonymous newanonym = new Anonymous();
            newanonym.VisitedTime = anonymousUser.VisitedTime;
            newanonym.VisitorGuid = Guid.NewGuid();
            _IAnonymousRepository.AddCustomer(newanonym);
            return Request.CreateResponse(HttpStatusCode.OK, newanonym);
        }

        private HttpResponseMessage GetUser(Guid anonymousId)
        {
            var anonymousUser = _IAnonymousRepository.GetCustomer(anonymousId);
            return Request.CreateResponse(HttpStatusCode.OK, anonymousUser);
        }
    }
}
