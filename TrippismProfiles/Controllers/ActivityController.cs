﻿using ExpressMapper;
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
        [Route("api/profile/activity/search")]
        public async Task<HttpResponseMessage> Save(SearchActivityViewModel searchActivityViewModel)
        {
            return await Task.Run(() => SaveSearch(searchActivityViewModel));
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