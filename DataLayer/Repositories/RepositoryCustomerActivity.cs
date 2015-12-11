using DataLayer.Entities;
using DataLayer.Repositories.Interfaces;

namespace DataLayer.Repositories
{
    public class RepositoryCustomerActivity : MongoDBRepository<CustomerActivity>,IRepositoryCustomerActivity
    {

    }
}
