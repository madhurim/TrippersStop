using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    /// <summary>
    /// Sabre FareForecastController Response
    /// </summary> 
    [ProtoContract]
    public class FareRange
    {
        [ProtoMember(1)]
        public string OriginLocation { get; set; }
        [ProtoMember(2)]
        public string DestinationLocation { get; set; }
        [ProtoMember(3)]
        public List<FareData> FareData { get; set; }

    }
}
