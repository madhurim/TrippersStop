using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public interface IRepository<T> where T : class //For GEneric Reppository
    {

        List<T> Get();//Select All data
        void Save(T entity);
        void Delete(string id);
        T GetById(string id);//Select by Id
        void Update(T entity);

    }
}
