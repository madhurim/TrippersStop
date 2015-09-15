using DataLayer.Entities;
using DataLayer.Repositories.Interfaces;

namespace DataLayer.Repositories
{
    public class RepositoryAuthDetails : MongoDBRepository<AuthDetails>, IRepositoryAuthDetails
    {
        
    }
}
