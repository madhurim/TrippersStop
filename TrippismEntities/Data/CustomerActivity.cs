//using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using DataLayer.Interfaces;

namespace TrippismEntities
{
    public class CustomerActivity : EntityBase
    {
        public Guid CustomerGuid { get; set; }
        public int ActivityTypeID { get; set; }
        public DateTime ActivityDate { get; set; }

        public enum ActivityType
        {
            Search = 1,
            Destinations = 2,
            Book = 3,
            Destination = 4
        }

    }
}
