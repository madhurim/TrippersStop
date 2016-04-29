using TrippismEntities;
using DataLayer;
using System;


namespace TrippismRepositories
{
    //TBD : Will be used when we separate Auth and customer 
    public class AccountRepository : IAccountRepository
    {
        //private IDBContext<Anonymous> _iDBContext;
        private IDBContext _iDBContext;
        public AccountRepository(IDBContext iDBContext)
        {
            this._iDBContext = iDBContext;
        }



        public Customer GetCustomer(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Customer UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Customer AddCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
