using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModel
{
    public class Destinations
    {
       
        public string Origin { get; set; }
        public string Earliestdeparturedate{ get; set; }
        public string Latestdeparturedate{ get; set; }
        public string Lengthofstay { get; set; }
        public string Theme { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public string Location { get; set; }
        public string Minfare { get; set; }
        public string Maxfare { get; set; }
        public string PointOfSaleCountry { get; set; }
        public string Region { get; set; }
        public string TopDestinations { get; set; }
        
    }
}
