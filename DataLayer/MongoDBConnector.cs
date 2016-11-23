using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using DataLayer;
using System.Configuration;
using EmailService;

namespace DataLayer
{
    public class MongoDBConnector : INoSqlConnector<IMongoDatabase>
    {
        private MongoClient _mongoClient;

        public IMongoDatabase connect()
        {
            MailgunEmail mail = new MailgunEmail();
            _mongoClient = new MongoClient(ConfigurationManager.AppSettings["MongoDBServer"]);
            if (_mongoClient.Cluster.Description.State == MongoDB.Driver.Core.Clusters.ClusterState.Disconnected)
            {
                List<string> listToaddress = new List<string>();
                listToaddress.Add("shubham4616@gmail.com");
                mail.SendComplexMessage("subham@trivenitechnologies.in", "MongoDB connection failed", listToaddress, "MongoDB is not connected.Please check your MongoDB connection");
            }
            return _mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongodbName"]);
        }
    }
}
