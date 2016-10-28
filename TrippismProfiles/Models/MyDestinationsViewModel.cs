using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippismProfiles.Models
{
    public class MyDestinationsViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerGuid { get; set; }
        public string Destination { get; set; }
        public Boolean LikeStatus { get; set; }
    }
}