using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataLayer.Interfaces
{
    public interface IDefaultLog
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public DateTime Date { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public Properties Properties { get; set; }
    }


    public class Properties
    {
        public int ThreadID { get; set; }
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string UserName { get; set; }
    }
}
