using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrippismProfiles.Models
{
    public class SearchActivityViewModel
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
      //  public int SearchType { get; set; }
        public string OriginAirPort { get; set; }
        public string DestinationAirPort { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int LengthOfStay { get; set; }
        public decimal? MaxFare { get; set; }
        public decimal? MinFate { get; set; }
        public string Theme { get; set; }
        public string Region { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}