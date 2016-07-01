using System;
using System.Collections.Generic;
using System.Linq;
using TraveLayer.CustomTypes.TripAdvisor.ViewModels;

namespace BusinessLogic
{
    public class TripAdvisorShopsAndSpasBusinessLayer : IBusinessLayer<LocationAttraction, LocationAttraction>
    {
        public LocationAttraction Process(LocationAttraction locations)
        {
            LocationAttraction locationAttraction = new LocationAttraction();
            locationAttraction.Attractions = new List<Attraction>();
            List<Attraction> attractionList = locations.Attractions.Where(x => x.Ranking != null && Convert.ToInt16(x.Ranking.Ranking) <= 10).ToList();
            if (!attractionList.Any())
                locationAttraction.Attractions = locations.Attractions.Where(x => x.Ranking != null).OrderBy(x => Convert.ToInt16(x.Ranking.Ranking)).ToList();
            return locationAttraction;
        }
    }
}
