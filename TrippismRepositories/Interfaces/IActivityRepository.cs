using DataLayer;
using System;
using System.Collections.Generic;
using TrippismEntities;

namespace TrippismRepositories
{
    public interface IActivityRepository 
    {
        SearchCriteria SaveSearch(SearchCriteria searchCriteria);
        List<SearchCriteria> FindSearch(Guid customerId);
        List<SearchCriteria> FindSearch(Guid customerId, int pageIndex, int pageSize);

        int GetCount(Guid customerId);

        DestinationsLike SaveLikes(DestinationsLike destinationLikes);
    }
}
