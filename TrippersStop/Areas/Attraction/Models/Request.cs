using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TraveLayer.CustomTypes.TripAdvisor.Request;

namespace Trippism.Areas.Attraction.Models
{
    public class Request : AttractionsRequest
    {

        public string GoogleCategories { get; set; }
        private List<TraveLayer.CustomTypes.TripAdvisor.Response.Datum> _TAResults;
        public List<TraveLayer.CustomTypes.TripAdvisor.Response.Datum> TAResults
        {
            get
            {
                if (_TAResults == null)
                {
                    _TAResults = new List<TraveLayer.CustomTypes.TripAdvisor.Response.Datum>();
                }
                return _TAResults;
            }
            set
            {
                _TAResults = value;

            }
        }

        private List<TraveLayer.CustomTypes.Google.Response.results> _GAResults;
        public List<TraveLayer.CustomTypes.Google.Response.results> GAResults { 
            
            get
            {
                if (_GAResults == null)
                {
                    _GAResults = new List<TraveLayer.CustomTypes.Google.Response.results>();
                }
                return _GAResults;
            }
            set
            {
                _GAResults = value;
            
            }

        
        }
    }
}