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
        public string OperatingSystem { get; set; }

    }
}