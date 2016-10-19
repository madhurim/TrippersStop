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

        MyDestinations SaveLikes(MyDestinations destinationLikes);

        List<MyDestinations> FindDestinationLikes(Guid customerId,string origin);
        int DeleteDestinationLikes(Guid customerId, string origin, string destination);
    }
}
