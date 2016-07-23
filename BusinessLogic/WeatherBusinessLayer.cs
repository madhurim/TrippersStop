using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TraveLayer.CustomTypes.Weather;

namespace BusinessLogic
{
    public class WeatherBusinessLayer : IBusinessLayer<Trip, TripWeather>
    {
        public TripWeather Process(Trip tripWeather)
        {
            TripWeather weather = new TripWeather();
            weather.WeatherChances = new List<WeatherChance>();
            Type type = tripWeather.chance_of.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var result = property.GetValue(tripWeather.chance_of, null);
                if (result != null && property.Name.IndexOf("tempover") != 0)
                {
                    Chance propertyValue = result as Chance;
                    bool addToCollection = IsValidRecord(propertyValue);
                    if (addToCollection)
                    {
                        WeatherChance weatherChanges = AddChanceOfRecord(propertyValue, property.Name);
                        weather.WeatherChances.Add(weatherChanges);
                    }
                }
            }
            return weather;
        }

        public static WeatherChance AddChanceOfRecord(Chance propertyValue, string propertyName)
        {
            WeatherChance weatherChanges = new WeatherChance()
            {
                ChanceType = propertyName,
                Description = propertyValue.description,
                Name = GetName(propertyValue.name),
                Percentage = propertyValue.percentage
            };
            return weatherChanges;
        }

        public static string GetName(string name)
        {
            switch (name)
            {
                case "Rain": return "Rainy";
                case "Sweltering": return "Very Hot";
                default: return name;
            }
        }
        public static bool IsValidRecord(Chance chance)
        {
            if (chance != null && chance.percentage >= 60)
            {
                return true;
            }
            return false;
        }
    }
}
