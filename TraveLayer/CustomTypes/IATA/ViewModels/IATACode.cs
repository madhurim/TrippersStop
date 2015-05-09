using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trippism.Areas.IATA.Models;

namespace TraveLayer.CustomTypes.IATA.ViewModels
{
    public class IATACode : Entity
    {
       // public string id { get; set; }
        public string CityCode { get; set; }
        public string CountryName { get; set; }
        public string IAName { get; set; }
    }
   
}
