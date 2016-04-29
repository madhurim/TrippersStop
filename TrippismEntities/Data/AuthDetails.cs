//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
using System;
//using DataLayer.Interfaces;

namespace TrippismEntities
{
    public class AuthDetails : EntityBase
    {

        public int AuthenticationType { get; set; }
        public string Password { get; set; }
        public Guid CustomerGuid { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsActive { get; set; }
        public Customer Customer { get; set; }

        public enum AuthType
        {
            Facebook = 1,
            Google = 2,
            Manual = 3,
        }
    }
}
