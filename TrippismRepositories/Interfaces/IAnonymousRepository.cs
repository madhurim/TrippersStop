using DataLayer;
using System;
using TrippismEntities;

namespace TrippismRepositories
{
    public interface IAnonymousRepository 
    {
        Anonymous GetCustomer(Guid customerId);
        Anonymous UpdateCustomer(Anonymous anonymousCustomer);
        Anonymous AddCustomer(Anonymous anonymousCustomer);
    }
}
