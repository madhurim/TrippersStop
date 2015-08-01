/// <summary>
///  This class  returns the median, highest, and lowest published fares that were ticketed via the Sabre GDS 
///  during the previous 4 weeks for each of the future departure dates in a range, using the specific origin, 
///  destination, and length of stay in the request.
/// </summary>

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    [ProtoContract]
    public class FareData
    {
        [ProtoMember(1)]
        public double MaximumFare { get; set; }
        [ProtoMember(2)]
        public double MinimumFare { get; set; }
        [ProtoMember(3)]
        public double MedianFare { get; set; }
        [ProtoMember(4)]
        public string CurrencyCode { get; set; }
        [ProtoMember(5)]
        public string Count { get; set; }
        [ProtoMember(6)]
        public string DepartureDateTime { get; set; }
        [ProtoMember(7)]
        public string ReturnDateTime { get; set; }
        [ProtoMember(8)]
        public List<Link> Links { get; set; }
    }

   
    public class OTA_FareRange
    {
        public string OriginLocation { get; set; }
        public string DestinationLocation { get; set; }
        public List<FareData> FareData { get; set; }
        public List<Link> Links { get; set; }
    }
    //TBD: Need to be clean 
    public class FareRange
    {
        public OTA_FareRange OTA_FareRange { get; set; }
        //public string OriginLocation { get; set; }
        //public string DestinationLocation { get; set; }
        //public List<FareData> FareData { get; set; }
    }

}
