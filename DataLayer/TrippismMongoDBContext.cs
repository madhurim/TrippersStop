using System;
using System.Configuration;

namespace DataLayer
{
    //   public class MongoDBContext<TObject> : IDBContext<TObject> where TObject : EntityBase   
    public class TrippismMongoDBContext : MongoDBAbstract
    {
        static string con = ConfigurationManager.AppSettings["MongoDBServer"];
        static string db = ConfigurationManager.AppSettings["MongoDBName"];
        public TrippismMongoDBContext()
            : base(con, db)
        {

        }
    }
}
