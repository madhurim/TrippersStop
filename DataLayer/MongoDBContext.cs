using MongoDB.Driver;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using System.Collections.Generic;
using System.Reflection;
using System.Configuration;
using DataLayer;

namespace DataLayer
{
     //   public class MongoDBContext<TObject> : IDBContext<TObject> where TObject : EntityBase
    public class MongoDBContext : IDBContext
    {
        MongoDBConnector mongoDBConnector = new MongoDBConnector();
        protected IMongoDatabase _db;

        public MongoDBContext()
        {
            _db = mongoDBConnector.connect();
           // CreateCollectionIfNotExist(typeof(TObject));
        }

        public List<T> Find<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            IFindFluent<T, T> results = _db.GetCollection<T>(typeof(T).Name).Find(expression == null ? (x => 1 == 1) : expression);
            return results.ToListAsync<T>().Result;
        }

        public T FindOne<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            IFindFluent<T, T> results = _db.GetCollection<T>(typeof(T).Name).Find(expression == null ? (x => 1 == 1) : expression);
            return results.ToListAsync<T>().Result.FirstOrDefault();
        }

        public List<T> Find<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression, int page, int pageSize) where T : class, new()
        {
            IFindFluent<T, T> results = _db.GetCollection<T>(typeof(T).Name).Find(expression == null ? (x => 1 == 1) : expression);
            results.Options.Skip = (pageSize * (page - 1));
            results.Options.Limit = pageSize;
            return results.ToListAsync<T>().Result;
        }

        public void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            _db.GetCollection<T>(typeof(T).Name).DeleteManyAsync(expression == null ? (x => 1 == 1) : expression);
        }

        public void Add<T>(T item) where T : class, new()
        {
            CreateCollectionIfNotExist(typeof(T));
            _db.GetCollection<T>(typeof(T).Name).InsertOneAsync(item);
        }

        public void AddMany<T>(IEnumerable<T> items) where T : class, new()
        {
            CreateCollectionIfNotExist(typeof(T));
            _db.GetCollection<T>(typeof(T).Name).InsertManyAsync(items);
        }

        public void Update<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression, T item) where T : class, new()
        {
            FilterDefinition<T> filters = Builders<T>.Filter.Where(expression == null ? (x => 1 == 1) : expression);

            Type itemType = item.GetType();
            List<BsonElement> bsonUpdateElements = new List<BsonElement>();

            foreach (PropertyInfo prop in itemType.GetProperties())
                bsonUpdateElements.Add(new BsonElement(prop.Name, BsonValue.Create(prop.GetValue(item, null))));

            BsonDocument bsonDoc = new BsonDocument(bsonUpdateElements);
            var bsonUpdateDoc = new BsonDocument { { "$set", bsonDoc } };
            _db.GetCollection<T>(typeof(T).Name).UpdateOneAsync(filters, bsonUpdateDoc);
        }

        public void UpdateWithChild<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression, T item) where T : class, new()
        {
            FilterDefinition<T> filters = Builders<T>.Filter.Where(expression == null ? (x => 1 == 1) : expression);
            BsonDocument bsonDoc = item.ToBsonDocument();
            var bsonUpdateDoc = new BsonDocument { { "$set", bsonDoc } };
            _db.GetCollection<T>(typeof(T).Name).UpdateOneAsync(filters, bsonUpdateDoc);
        }

        public void UpdateMany<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression, BsonDocument bsonUpdateDoc) where T : class, new()
        {
            FilterDefinition<T> filters = Builders<T>.Filter.Where(expression == null ? (x => 1 == 1) : expression);
            _db.GetCollection<T>(typeof(T).Name).UpdateManyAsync(filters, bsonUpdateDoc);
        }

        private void CreateCollectionIfNotExist(Type T)
        {
            int count = _db.ListCollectionsAsync().Result.ToListAsync().Result.Where(x => x["name"] == T.Name).Count();
            if (count == 0) _db.CreateCollectionAsync(T.Name);
        }

        public List<T> All<T>() where T : class, new()
        {
            IFindFluent<T, T> results = _db.GetCollection<T>(typeof(T).Name).Find(null);
            return results.ToListAsync<T>().Result;
        }

        public long Count<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            IFindFluent<T, T> results = _db.GetCollection<T>(typeof(T).Name).Find(expression == null ? (x => 1 == 1) : expression);
            return results.CountAsync().Result;
        }

        public void Dispose()
        {

        }

    }
}
