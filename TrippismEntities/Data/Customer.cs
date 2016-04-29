//using MongoDB.Bson;
using System;
//using DataLayer.Interfaces;
//using MongoDB.Bson.Serialization.Attributes;

namespace TrippismEntities
{
    public class Customer : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }

        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
