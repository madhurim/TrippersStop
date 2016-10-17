﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippismProfiles.Models
{
    public class DestinationLikesViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerGuid { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
    }
}