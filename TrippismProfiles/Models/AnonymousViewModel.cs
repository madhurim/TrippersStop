using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippismProfiles.Models
{
    public class AnonymousViewModel
    {
        public Guid VisitorGuid { get; set; }
        public DateTime VisitedTime { get; set; }
        public string Device { get; set; }
        public string Browser { get; set; }
        public string Ipaddress { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string OperatingSystem { get; set; }

    }
}