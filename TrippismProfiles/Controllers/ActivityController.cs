using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TrippismApi.TraveLayer;
using TrippismEntities;
using TrippismProfiles.Models;
using TrippismRepositories;

namespace TrippismProfiles.Controllers
{

    /// <summary>
    /// It's used to save and get customer activity.
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class ActivityController : ApiController
    {
        IActivityRepository _IActivityRepository;
        ICacheService _cacheService;

        /// <summary>
        /// It's used to initialize repository.
        /// </summary>
        public ActivityController(ICacheService cacheService, IActivityRepository iActivityRepository)
        {
            _IActivityRepository = iActivityRepository;
            _cacheService = cacheService;
        }

        /// <summary>
        /// It's used to save customer seach activity.
        /// </summary>
        [HttpPost]
        [Route("api/profile/activity/save")]
        public async Task<HttpResponseMessage> Save(SearchActivityViewModel searchActivityViewModel)
        {
            return await Task.Run(() => SaveSearch(searchActivityViewModel));
        }


        /// <summary>
        /// It's used to save customer destinations likes.
        /// </summary>
        [HttpPost]
        [Route("api/profile/activity/destinationLikes")]
        public async Task<HttpResponseMessage> LikesDestinations(MyDestinationsViewModel destinationLikesViewModel)
        {
            return await Task.Run(() => SaveLikes(destinationLikesViewModel));
        }


        /// <summary>
        /// It's used to delete customer destinations likes.
        /// </summary>
        [HttpPost]
        [Route("api/profile/activity/deleteDestinationLikes")]
        public async Task<HttpResponseMessage> DeleteLikesDestinations(MyDestinationsViewModel destinationLikesViewModel)
        {
            return await Task.Run(() => DeleteLikes(destinationLikesViewModel));
        }

        /// <summary>
        /// It's used to getting customer list of destinations likes.
        /// </summary>
        [HttpGet]
        [Route("api/profile/activity/getdestinationLikes")]
        public async Task<HttpResponseMessage> GetLikesDestinations(Guid customerId, string origin)
        {
            return await Task.Run(() => GetLikes(customerId, origin));
        }
        /// <summary>
        /// It's used to get customer seach activity.
        /// </summary>
        [Route("api/profile/activity/search/all")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllSearch(Guid customerId)
        {
            return await Task.Run(() => GetSearch(customerId));
        }

        /// <summary>
        /// It's used to get customer seach activity by page index and page size.
        /// </summary>
        [Route("api/profile/activity/search")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetSearchPageWise(Guid customerId, int pageNo, int pageSize)
        {
            return await Task.Run(() => GetSearch(customerId, pageNo, pageSize));
        }
        #region local Method


        private HttpResponseMessage SaveSearch(SearchActivityViewModel searchActivityViewModel)
        {
            string message = string.Empty;
            searchActivityViewModel.CreatedDate = DateTime.Now;
            var searchCriteria = Mapper.Map<SearchActivityViewModel, SearchCriteria>(searchActivityViewModel);
            _IActivityRepository.SaveSearch(searchCriteria);
            return Request.CreateResponse(HttpStatusCode.OK, searchCriteria.Id);
        }

        private HttpResponseMessage SaveLikes(MyDestinationsViewModel MyDestinationssViewModel)
        {
            string message = string.Empty;
            var destinationLikes = Mapper.Map<MyDestinationsViewModel, MyDestinations>(MyDestinationssViewModel);
            _IActivityRepository.SaveLikes(destinationLikes);
            return Request.CreateResponse(HttpStatusCode.OK, destinationLikes);
        }
        private HttpResponseMessage DeleteLikes(MyDestinationsViewModel MyDestinationssViewModel)
        {
            _IActivityRepository.DeleteDestinationLikes(MyDestinationssViewModel.CustomerGuid,MyDestinationssViewModel.Origin,MyDestinationssViewModel.Destination);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private HttpResponseMessage GetLikes(Guid customerId, string origin)
        {
            var SearchData = _IActivityRepository.FindDestinationLikes(customerId, origin);
            return Request.CreateResponse(HttpStatusCode.OK, SearchData);
        }

        private HttpResponseMessage GetSearch(Guid customerId)
        {
            var SearchData = _IActivityRepository.FindSearch(customerId);
            return Request.CreateResponse(HttpStatusCode.OK, SearchData);
        }
        private HttpResponseMessage GetSearch(Guid customerId, int pageIndex, int pageSize)
        {
            var searchList = _IActivityRepository.FindSearch(customerId, pageIndex, pageSize);
            return Request.CreateResponse(HttpStatusCode.OK, searchList);
        }
        #endregion
    }
}
