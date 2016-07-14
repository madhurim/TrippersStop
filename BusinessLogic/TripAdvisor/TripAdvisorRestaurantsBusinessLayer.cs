using System;
using System.Collections.Generic;
using System.Linq;
using TraveLayer.CustomTypes.TripAdvisor.ViewModels;

namespace BusinessLogic
{
    public class TripAdvisorRestaurantsBusinessLayer : ITripAdvisorRestaurantsBusinessLayer<LocationAttraction, LocationAttraction>
    {
        public LocationAttraction Process(LocationAttraction locations)
        {
            LocationAttraction locationAttraction = new LocationAttraction();
            locationAttraction.Attractions = new List<Attraction>();
            List<Attraction> attractionList = locations.Attractions.Where(x => x.Rating != null && x.Ranking != null && Convert.ToInt16(x.Ranking.Ranking) <= 10).ToList();
            locationAttraction.Attractions = attractionList;
            return locationAttraction;
        }
    }
}
