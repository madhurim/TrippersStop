using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using DataLayer.Interfaces;
using System.Configuration;

namespace DataLayer
{
    public class NoSqlConnector : INoSqlConnector
    {
        private MongoClient _mongoClient;

        public IMongoDatabase connect()
        {
            _mongoClient = new MongoClient(ConfigurationManager.AppSettings["MongoDBServer"]);
            return _mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongoDBName"]);
        }
    }
}
