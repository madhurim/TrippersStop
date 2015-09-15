using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Interfaces;

namespace DataLayer.Entities
{
    public class AuthDetails : IAuthDetails
    {
        public enum AuthType
        {
            Facebook = 1,
            Google = 2,
            Manual = 3,
        }
        [BsonId]
        public ObjectId _id { get; set; }
        public int AuthenticationType { get; set; }
        public string Password { get; set; }
        public Guid CustomerGuid { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        //LoggedIn
        public DateTime LoginTime { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
