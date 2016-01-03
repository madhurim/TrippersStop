using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.ViewModels
{
    public class Award
    {
        public string AwardType { get; set; }
        public string Year { get; set; }
        public Images Images { get; set; }
        public List<object> Categories { get; set; }
        public string DisplayName { get; set; }
    }
}
