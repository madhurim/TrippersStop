using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Weather;

namespace BusinessLogic
{
    public class WeatherBusinessLayer : IBusinessLayer<Trip, TripWeather>
    {
        public TripWeather Process(Trip tripWeather)
        {
            TripWeather weather = new TripWeather();
            Type type = tripWeather.chance_of.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string propertyName = string.Empty;
            Chance propertyValue = null;
            string separator = string.Empty;
            foreach (PropertyInfo property in properties)
            {
                propertyName = property.Name;
                var result = property.GetValue(tripWeather.chance_of, null);
                if (result != null)
                {
                    propertyValue = result as Chance;
                    bool addToCollection = IsValidRecord(propertyValue);
                    if (addToCollection)
                       weather =  FilterChanceRecord(tripWeather, weather);
                }
            }

            return weather;
        }
        private TripWeather FilterChanceRecord(Trip trip, TripWeather tripWeather)
        {
            if (trip.chance_of.chanceofcloudyday != null && IsValidRecord(trip.chance_of.chanceofcloudyday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofcloudyday, "chanceofcloudyday");
            }
            if (trip.chance_of.chanceoffogday != null && IsValidRecord(trip.chance_of.chanceoffogday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceoffogday, "chanceoffogday");
            }
            if (trip.chance_of.chanceofhailday != null && IsValidRecord(trip.chance_of.chanceofhailday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofhailday, "chanceofhailday");
            }
            if (trip.chance_of.chanceofhumidday != null && IsValidRecord(trip.chance_of.chanceofhumidday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofhumidday, "chanceofhumidday");
            }
            if (trip.chance_of.chanceofpartlycloudyday != null && IsValidRecord(trip.chance_of.chanceofpartlycloudyday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofpartlycloudyday, "chanceofpartlycloudyday");
            }
            if (trip.chance_of.chanceofprecip != null && IsValidRecord(trip.chance_of.chanceofprecip))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofprecip, "chanceofprecip");
            }
            if (trip.chance_of.chanceofrainday != null && IsValidRecord(trip.chance_of.chanceofrainday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofrainday, "chanceofrainday");
            }
            if (trip.chance_of.chanceofsnowday != null && IsValidRecord(trip.chance_of.chanceofsnowday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofsnowday, "chanceofsnowday");
            }
            if (trip.chance_of.chanceofsnowonground != null && IsValidRecord(trip.chance_of.chanceofsnowonground))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofsnowonground, "chanceofsnowonground");
            }
            if (trip.chance_of.chanceofsultryday != null && IsValidRecord(trip.chance_of.chanceofsultryday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofsultryday, "chanceofsultryday");
            }
            if (trip.chance_of.chanceofsunnycloudyday != null && IsValidRecord(trip.chance_of.chanceofsunnycloudyday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofsunnycloudyday, "chanceofsunnycloudyday");
            }
            if (trip.chance_of.chanceofthunderday != null && IsValidRecord(trip.chance_of.chanceofthunderday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofthunderday, "chanceofthunderday");
            }
            if (trip.chance_of.chanceoftornadoday != null && IsValidRecord(trip.chance_of.chanceoftornadoday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceoftornadoday, "chanceoftornadoday");
            }
            if (trip.chance_of.chanceofwindyday != null && IsValidRecord(trip.chance_of.chanceofwindyday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofwindyday, "chanceofwindyday");
            }

            return tripWeather;
        }


        public static void AddChanceOfRecord(TripWeather tripWeather, Chance propertyValue, string propertyName)
        {
            tripWeather.WeatherChances = new List<WeatherChance>();
            tripWeather.WeatherChances.Add(new WeatherChance()
            {
                ChanceType = propertyName,
                Description = propertyValue.description,
                Name = propertyValue.name,
                Percentage = propertyValue.percentage
            });
        }

        public static bool IsValidRecord(Chance chance)
        {
            if (chance != null && chance.percentage > 30)
            {
                return true;
            }
            return false;
        }
    }
}
