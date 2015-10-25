using DataLayer.Entities;
using DataLayer.Repositories.Interfaces;

namespace DataLayer.Repositories
{
    public class RepositoryAnonymous : MongoDBRepository<Anonymous>, IRepositoryAnonymous
    {
       
    }
}
