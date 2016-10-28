using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrippismEntities;

namespace TrippismRepositories
{
    public class ActivityRepository : IActivityRepository
    {
        private IDBContext _iDBContext;
        public ActivityRepository(IDBContext iDBContext)
        {
            this._iDBContext = iDBContext;
        }
        public SearchCriteria SaveSearch(SearchCriteria searchCriteria)
        {
            _iDBContext.Add<SearchCriteria>(searchCriteria);
            return searchCriteria;
        }
        public MyDestinations SaveLikes(MyDestinations destinationLikes)
        {
            _iDBContext.Add<MyDestinations>(destinationLikes);
            return destinationLikes;
        }
        public List<SearchCriteria> FindSearch(Guid customerId)
        {
            var searchList = _iDBContext.Find<SearchCriteria>(x => x.RefGuid == customerId).ToList();
            return searchList;
        }

        public List<SearchCriteria> FindSearch(Guid customerId, int pageIndex, int pageSize)
        {
            var searchList = _iDBContext.Find<SearchCriteria>(x => x.RefGuid == customerId, pageIndex, pageSize).ToList();
            return searchList;
        }

        public List<MyDestinations> FindDestinationLikesList(Guid customerId)
        {
            var searchList = _iDBContext.Find<MyDestinations>(x => x.CustomerGuid == customerId).ToList();
            return searchList;
        }
        public MyDestinations FindDestinationLikes(Guid customerId, string destination)
       {
            var searchList = _iDBContext.Find<MyDestinations>(x => x.CustomerGuid == customerId && x.Destination == destination).FirstOrDefault();
            return searchList;
        }
        public MyDestinations UpdateDestinationLikes(MyDestinations myDestination)
        {
            _iDBContext.Update<MyDestinations>(a => a.CustomerGuid == myDestination.CustomerGuid && a.Destination == myDestination.Destination, myDestination);
            return myDestination;
        }
        public int GetCount(Guid customerId)
        {
            var count = _iDBContext.Find<SearchCriteria>(x => x.RefGuid == customerId).Count;
            return count;
        }

        public int DeleteDestinationLikes(Guid customerId, string destination)
        {
            _iDBContext.Delete<MyDestinations>(x => x.CustomerGuid == customerId && x.Destination == destination);
            return 1;
        }
    }
}
