using TrippismEntities;
using DataLayer;
using System;


namespace TrippismRepositories
{

    public class AnonymousRepository : IAnonymousRepository
    {
        //private IDBContext<Anonymous> _iDBContext;
        private IDBContext _iDBContext;
        public AnonymousRepository(IDBContext iDBContext)
        {
            this._iDBContext = iDBContext;
        }

        public Anonymous GetCustomer(Guid anonymousId)
        {
            var customer = _iDBContext.FindOne<Anonymous>(a => a.VisitorGuid == anonymousId);
            return customer;
        }

        public Anonymous UpdateCustomer(Anonymous anonymousCustomer)
        {
            var customer = _iDBContext.FindOne<Anonymous>(a => a.VisitorGuid == anonymousCustomer.VisitorGuid);
            if (customer == null)
            {
                return null;
            }
            _iDBContext.Update<Anonymous>(null, customer);
            return customer;
        }

        public Anonymous AddCustomer(Anonymous anonymousCustomer)
        {
            if (anonymousCustomer == null)
                return null;
            _iDBContext.Add<Anonymous>(anonymousCustomer);
            return anonymousCustomer;
        }
    }
}
