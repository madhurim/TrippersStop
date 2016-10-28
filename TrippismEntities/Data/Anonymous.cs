//using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using DataLayer.Interfaces;

namespace TrippismEntities
{
    public class Anonymous : EntityBase
    {
        public Guid VisitorGuid { get; set; }
        public DateTime VisitedTime { get; set; }

        // MM : We perhaps don't need this - commented out
        //public Guid KnownGuid { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public string MobileDeviceModel { get; set; }
        public string InputType { get; set; }
        public string MobileDeviceManufacturer { get; set; }
        public string Ipaddress { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}
