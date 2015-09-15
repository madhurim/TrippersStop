using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Interfaces;

namespace DataLayer.Entities
{
    public class Customer :ICustomer
    {
       public ObjectId _id { get; set; }
       public Guid CustomerGuid { get; set; }
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public DateTime DOB { get; set; }
       public string Gender { get; set; }
       public string Mobile { get; set; }
       public DateTime CreatedDate { get; set; }
       public DateTime ModifiedDate { get; set; }
    }
}
