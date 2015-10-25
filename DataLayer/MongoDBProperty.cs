using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace DataLayer
{
    public static class MongoDBProperty
    {
        public static MongoDatabase _mongoDB;
        public static MongoDatabase MongoDB
        {
            get { return _mongoDB; }
            set { _mongoDB = value; }
        }
    }
}
