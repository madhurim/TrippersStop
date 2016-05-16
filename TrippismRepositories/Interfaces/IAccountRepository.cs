using DataLayer;
using System;
using TrippismEntities;

namespace TrippismRepositories
{
    public interface IAccountRepository 
    {
        Customer GetCustomer(Guid customerId);
        Customer UpdateCustomer(Customer customer);
        Customer AddCustomer(Customer customer);
    }
}
