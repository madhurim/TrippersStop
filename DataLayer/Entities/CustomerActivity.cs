using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Interfaces;

namespace DataLayer.Entities
{
    public class CustomerActivity : ICustomerActivity
    {
        public ObjectId _id { get; set; }
        public Guid CustomerGuid { get; set; }
        public int ActivityTypeID { get; set; }
        public DateTime ActivityDate { get; set; }

        public enum ActivityType
        {
            Search = 1,
            Destinations = 2,
            BargainPrices = 3,
        }

    }
}
