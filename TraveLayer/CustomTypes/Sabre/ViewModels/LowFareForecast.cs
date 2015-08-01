using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    /// <summary>
    /// Sabre LowFareForecast response
    /// </summary> 
    [ProtoContract]
    public class LowFareForecast
    {
        [ProtoMember(1)]
        public string OriginLocation { get; set; }
        [ProtoMember(2)]
        public string DestinationLocation { get; set; }
        [ProtoMember(3)]
        public string DepartureDateTime { get; set; }
        [ProtoMember(4)]
        public string ReturnDateTime { get; set; }
        [ProtoMember(5)]
        public Forecast Forecast { get; set; }
        [ProtoMember(6)]
        public string Recommendation { get; set; }
        [ProtoMember(7)]
        public double LowestFare { get; set; }
        [ProtoMember(8)]
        public string CurrencyCode { get; set; }
 
    }
}
