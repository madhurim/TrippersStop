using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataLayer.Interfaces
{
    public interface IDefaultLog
    {
        [BsonId]
        ObjectId _id { get; set; }
        DateTime Date { get; set; }
        string Level { get; set; }
        string Logger { get; set; }
        string Message { get; set; }
    }
}
