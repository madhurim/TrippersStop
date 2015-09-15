using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Interfaces;

namespace DataLayer.Entities
{
    public class Anonymous : IAnonymous
    {
        public ObjectId _id { get; set; }
        public Guid VisitorGuid { get; set; }
        public DateTime VisitedTime { get; set; }
        public Guid KnownGuid { get; set; }
    }
}
