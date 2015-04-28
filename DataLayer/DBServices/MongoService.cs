using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using System.Web.Configuration;


namespace DataLayer
{
    public class MongoService : IDBService
    {
        MongoServer server;
        //MongoDatabase database;
        MongoCollection collection;
       // NoSqlConnector connection = new NoSqlConnector();
        MongoDatabase database = MongoDBProperty.MongoDB;  //Using MongoDatabase properties 

        public string MongoHost
        {
            get
            {
                return WebConfigurationManager.AppSettings["MongoDBServer"];
            }
        }
        public  MongoService()
        {
            MongoClient client = new MongoClient(MongoHost);
            server = client.GetServer();
            database = server.GetDatabase("test");
            MongoDBProperty.MongoDB = database;
        }
        public List<T> Get<T>(string collectionName)
        {
            collection = database.GetCollection<T>(collectionName);
            var details = collection.FindAllAs<T>().ToList();
            return details;
        }

        public void Save<T>(T entity, string collectionName)
        {
            collection = database.GetCollection<T>(collectionName);
            collection.Insert(entity);
        }

        public void Delete<T>(string id, string collectionName)
        {
            collection = database.GetCollection<T>(collectionName);
            IMongoQuery query = Query.EQ("ID", id);
            collection.Remove(query);
        }

        //public T GetById<T>(string id)
        //{
        //    //T a = new T();
        //    //var collection = database.GetCollection<Anonymous>("Anonymous");
        //    //var entityQuery = Query<Anonymous>.EQ(e => e.Id, new ObjectId(id));
        //    //var get = collection.FindOne(entityQuery);
        //    //a.Name = get.ToString();
        //    return null;
        //}

        //public void Update<T>(T entity, string collectionName)
        //{
        //    collection = database.GetCollection<T>("Anonymous");
        //    IMongoQuery query = Query.EQ("ID", entity.Id);
        //    IMongoUpdate updateQuery = MongoDB.Driver.Builders.Update.Set("Name", entity.Name).Set("VisitedTime", entity.VisitedTime);
        //    collection.Update(query, updateQuery); 
        //}
    }
}