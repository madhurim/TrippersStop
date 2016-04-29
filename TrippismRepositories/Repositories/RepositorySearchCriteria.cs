using TrippismEntities;
using DataLayer;


namespace TrippismRepositories
{
    public class RepositorySearchCriteria : MongoDBContext<SearchCriteria>, IRepositorySearchCriteria
    {
    }
}
