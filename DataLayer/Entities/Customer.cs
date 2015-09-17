﻿using MongoDB.Bson;
using System;
using DataLayer.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer.Entities
{
    public class Customer : ICustomer
    {
        [BsonId]
        public ObjectId _id { get; set; }
        //public Guid CustomerGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
