//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
using System;
//using DataLayer.Interfaces;

namespace TrippismEntities
{
    public class EmailTemplate : EntityBase
    {
        public string EmailTemplateName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
