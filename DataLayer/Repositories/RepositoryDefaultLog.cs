using DataLayer.Entities;
using DataLayer.Repositories.Interfaces;

namespace DataLayer.Repositories
{
    public class RepositoryDefaultLog : MongoDBRepository<DefaultLog>, IRepositoryDefaultLog
    {

    }
}
