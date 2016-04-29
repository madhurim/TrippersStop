using DataLayer;
using System;
using TrippismEntities;

namespace TrippismRepositories
{
    public interface IAuthDetailsRepository
    {
        AuthDetails UpdateCustomer(AuthDetails authDetails);
        AuthDetails AddCustomer(AuthDetails authDetails);
        AuthDetails FindCustomer(string userName);
        AuthDetails FindCustomer(Guid authId);
    }
}
