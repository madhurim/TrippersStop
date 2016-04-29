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

        public List<SearchCriteria> FindSearch(Guid customerId)
        {
            var searchList = _iDBContext.Find<SearchCriteria>(x => x.RefGuid == customerId).ToList();
            return searchList;
        }

        public List<SearchCriteria> FindSearch(Guid customerId, int pageIndex, int pageSize)
        {
            var searchList = _iDBContext.Find<SearchCriteria>(x => x.RefGuid ==  customerId, pageIndex, pageSize).ToList();
            return searchList;
        }


        public int GetCount(Guid customerId)
        {
            var count = _iDBContext.Find<SearchCriteria>(x => x.RefGuid == customerId).Count;
            return count;
        }
    }
}
