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

        //public string Ipaddress { get ; set ; }
        //public string OperatingSystem { get ; set ; }
        //public string Location { get ; set ; }
        
    }
}
