using System;
using System.Configuration;

namespace DataLayer
{
    public class TrippismTemplateMongoDBContext : MongoDBAbstract, IEmailContext
    {
        static string con = ConfigurationManager.AppSettings["EmailTemplateMongoDBServer"];
        static string db = ConfigurationManager.AppSettings["EmailTemplateMongoDBName"];

        public TrippismTemplateMongoDBContext()
            : base(con, db)
        {

        }

    }
}
