using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DataLayer
{
    interface IDBService
    {
        //void connect();

        List<T> Get<T>(string collectionName);//Select All data

        void Save<T>(T entity, string collectionName);

        void Delete<T>(string id, string collectionName);

        // T GetById<T>(string id);//Select by Id

        //void Update<T>(T entity);
    }
}
