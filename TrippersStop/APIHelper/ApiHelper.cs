using ExpressMapper;
using System.Configuration;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Weather;
using TrippismApi.TraveLayer;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Constants.ViewModels;
using TraveLayer.CustomTypes.Constants.Response;
using System.Linq;
using TAVM=TraveLayer.CustomTypes.TripAdvisor.ViewModels;
using RS=TraveLayer.CustomTypes.TripAdvisor.Response;
using Trippism.APIHelper;

namespace TrippismApi
{
    public static class ApiHelper
    {
        public static void SetApiToken(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            apiCaller.LongTermToken = cacheService.GetByKey<string>(apiCaller.SabreTokenKey);
            apiCaller.TokenExpireIn = cacheService.GetByKey<string>(apiCaller.SabreTokenExpireKey);
            if (string.IsNullOrWhiteSpace(apiCaller.LongTermToken))
            {
                string sabreAuthenticationUrl = ConfigurationManager.AppSettings["SabreAuthenticationUrl"];
                apiCaller.LongTermToken = apiCaller.GetToken(sabreAuthenticationUrl).Result;
                SaveTokenInCache(apiCaller, cacheService);
            }
            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }

        private static void SaveTokenInCache(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            double expireTimeInSec;
            if (!string.IsNullOrWhiteSpace(apiCaller.TokenExpireIn) && double.TryParse(apiCaller.TokenExpireIn, out expireTimeInSec))
            {
                cacheService.Save<string>(apiCaller.SabreTokenKey, apiCaller.LongTermToken, expireTimeInSec / 60);
                cacheService.Save<string>(apiCaller.SabreTokenExpireKey, apiCaller.TokenExpireIn, expireTimeInSec / 60);
            }
        }

        public static void RefreshApiToken(ICacheService _cacheService, IAsyncSabreAPICaller _apiCaller)
        {
            _cacheService.Expire(_apiCaller.SabreTokenKey);
            _cacheService.Expire(_apiCaller.SabreTokenExpireKey);
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
        }

        public static void FilterChanceRecord(Trip trip, TripWeather tripWeather)
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
        }

        public static void AddChanceOfRecord(TripWeather tripWeather, Chance propertyValue, string propertyName)
        {
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

        public static void RegisterMappingEntities()
        {
            // ConfigurationManager.AppSettings["SabreDestinationsUrl"];
            //Mapper.Register<InstaFlightsSearchOutput, InstaFlightSearch>();

            //Mapper.Register<PricedItinerary, PricedItineraryViewModel>();
            //Mapper.Register<AirItinerary, AirItineraryViewModel>();
            //Mapper.Register<FlightSegment, FlightSegmentViewModel>();
            //Mapper.Register<OriginDestinationOptions, OriginDestinationOptionsViewModel>();
            //Mapper.Register<OriginDestinationOption, OriginDestinationOptionViewModel>();
            //Mapper.Register<ItinTotalFare, ItinTotalFareViewModel>();
            //Mapper.Register<PTCFareBreakdowns, PTCFareBreakdownsViewModel>();
            //Mapper.Register<FareInfo, FareInfoViewModel>();
            //Mapper.Register<AirItineraryPricingInfo, AirItineraryPricingInfoViewModel>();
            //Mapper.Register<PTCFareBreakdown, PTCFareBreakdownViewModel>();
            //Mapper.Register<InstaFlightsSearchOutput, InstaFlightSearch>();

            Mapper.Register<OTA_DestinationFinder, Fares>();
            Mapper.Register<OTA_FareRange, VM.FareRange>();
            Mapper.Register<OTA_TravelSeasonality, VM.TravelSeasonality>();
            Mapper.Register<OTA_LowFareForecast, LowFareForecast>();
            Mapper.Register<TempHigh, TempHighAvg>()
                  .Member(h => h.Avg, m => m.avg);
            Mapper.Register<TempLow, TempLowAvg>()
               .Member(h => h.Avg, m => m.avg);
            Mapper.Register<Trip, TripWeather>()
            .Member(h => h.TempHighAvg, m => m.temp_high)
            .Member(h => h.TempLowAvg, m => m.temp_low)
            .Member(h => h.CloudCover, m => m.cloud_cover);
            Mapper.Register<CurrencySymbols, CurrencySymbolsViewModel>()
                .Member(h => h.Currency, m => m.Currencies.Currency);
            Mapper.Compile();
        }


        public static void RegisterTripAdvisorMapping()
        {

            Mapper.Register<RS.AttractionType, TAVM.AttractionType>()
               .Member(h => h.Name, m => m.name)
               .Member(h => h.LocalizedName, m => m.localized_name);

            Mapper.Register<RS.Category, TAVM.Category>()
               .Member(h => h.Name, m => m.name)
               .Member(h => h.LocalizedName, m => m.localized_name);

            Mapper.Register<RS.RankingData, TAVM.Rank>()
               .Member(h => h.RankingInfo, m => m.ranking_string)
               .Member(h => h.RankingOutOf, m => m.ranking_out_of)
               .Member(h => h.GeoLocationId, m => m.geo_location_id)
               .Member(h => h.Ranking, m => m.ranking)
               .Member(h => h.GeoLocationName, m => m.geo_location_name);

            Mapper.Register<RS.AddressObj, TAVM.Address>()
                .Member(h => h.Street1, m => m.street1)
                .Member(h => h.Street2, m => m.street2)
                .Member(h => h.City, m => m.city)
                .Member(h => h.State, m => m.state)
                .Member(h => h.Country, m => m.country)
                .Member(h => h.PostalCode, m => m.postalcode);

            Mapper.Register<RS.WikipediaInfo, TAVM.WikiPediaInfo>()
                .Member(h => h.Language, m => m.language)
                .Member(h => h.Url, m => m.url)
                .Member(h => h.PageId, m => m.pageid);

            //

            Mapper.Register<RS.Images, TAVM.Images>()
             .Member(h => h.Large, m => m.large)
             .Member(h => h.Small, m => m.small);

            Mapper.Register<RS.UserLocation, TAVM.UserLocation>()
           .Member(h => h.Id, m => m.id)
           .Member(h => h.Name, m => m.name);

            Mapper.Register<RS.User, TAVM.User>()
             .Member(h => h.UserLocation, m => m.user_location)
             .Member(h => h.UserName, m => m.username);


          

            Mapper.Register<RS.Award, TAVM.Award>()
               .Member(h => h.AwardType, m => m.award_type)
               .Member(h => h.Categories, m => m.categories)
               .Member(h => h.DisplayName, m => m.display_name)
               .Member(h => h.Images, m => m.images)
               .Member(h => h.Year, m => m.year);

            Mapper.Register<RS.TripType, TAVM.TripType>()
           .Member(h => h.LocalizedName, m => m.localized_name)
           .Member(h => h.Name, m => m.name)
           .Member(h => h.Value, m => m.value);


            Mapper.Register<RS.Subrating, TAVM.Subrating>()
           .Member(h => h.LocalizedName, m => m.localized_name)
           .Member(h => h.Name, m => m.name)
           .Member(h => h.RatingImageUrl, m => m.rating_image_url)
           .Member(h => h.Value, m => m.value);

            Mapper.Register<RS.ReviewRatingCount, TAVM.ReviewRatingCount>()
        .Member(h => h.__invalid_name__1, m => m.__invalid_name__1)
        .Member(h => h.__invalid_name__2, m => m.__invalid_name__2)
        .Member(h => h.__invalid_name__3, m => m.__invalid_name__3)
            .Member(h => h.__invalid_name__4, m => m.__invalid_name__4)
        .Member(h => h.__invalid_name__5, m => m.__invalid_name__5);


            Mapper.Register<RS.Review, TAVM.Review>()
               .Member(h => h.HelpfulVotes, m => m.helpful_votes)
               .Member(h => h.Id, m => m.id)
               .Member(h => h.IsMachineTranslated, m => m.is_machine_translated)
               .Member(h => h.Lang, m => m.lang)
               .Member(h => h.LocationId, m => m.location_id)
                  .Member(h => h.PublishedDate, m => m.published_date)
               .Member(h => h.Rating, m => m.rating)
               .Member(h => h.RatingImageUrl, m => m.rating_image_url)
                  .Member(h => h.Text, m => m.text)
               .Member(h => h.Title, m => m.title)
               .Member(h => h.TravelDate, m => m.travel_date)
                  .Member(h => h.TripType, m => m.trip_type)
               .Member(h => h.Url, m => m.url)
               .Member(h => h.User, m => m.user);


            //
            Mapper.Register<RS.Datum, TAVM.Attraction>()
                .Member(h => h.Address, m => m.address_obj)
                .Member(h => h.Distance, m => m.distance)
                .Member(h => h.RecommendedPercentage, m => m.percent_recommended)
                .Member(h => h.Latitude, m => m.latitude)
                .Member(h => h.Rating, m => m.rating)
                .Member(h => h.LocationId, m => m.location_id)
                .Member(h => h.Ranking, m => m.ranking_data)      
                .Member(h => h.Location, m => m.location_string)
                .Member(h => h.WebUrl, m => m.web_url)
                .Member(h => h.PriceLevel, m => m.price_level)
                .Member(h => h.RatingImageUrl, m => m.rating_image_url)
                .Member(h => h.Name, m => m.name)
                .Member(h => h.NumReviews, m => m.num_reviews)
                .Member(h => h.Category, m => m.category)
                .Member(h => h.SeeAllPhotos, m => m.see_all_photos)
                .Member(h => h.Longitude, m => m.longitude)
                .Member(h => h.Cuisine, m => m.cuisine)
                .Member(h => h.WikiPediaInfo, m => m.wikipedia_info)
                .Member(h => h.AttractionTypes, m => m.attraction_types);
            Mapper.Register<RS.LocationInfo, TAVM.LocationAttraction>()
                .Member(h => h.Attractions, m => m.data);
            Mapper.Register<RS.Datum, TAVM.Location>()
                  .Member(h => h.Address, m => m.address_obj)
                .Member(h => h.Distance, m => m.distance)
                .Member(h => h.RecommendedPercentage, m => m.percent_recommended)
                .Member(h => h.Latitude, m => m.latitude)
                .Member(h => h.Rating, m => m.rating)
                .Member(h => h.LocationId, m => m.location_id)
                .Member(h => h.Ranking, m => m.ranking_data)
                .Member(h => h.Location, m => m.location_string)
                .Member(h => h.WebUrl, m => m.web_url)
                .Member(h => h.PriceLevel, m => m.price_level)
                .Member(h => h.RatingImageUrl, m => m.rating_image_url)
                .Member(h => h.Name, m => m.name)
                .Member(h => h.NumReviews, m => m.num_reviews)
                .Member(h => h.Category, m => m.category)
                .Member(h => h.SeeAllPhotos, m => m.see_all_photos)
                .Member(h => h.Longitude, m => m.longitude)
                .Member(h => h.Cuisine, m => m.cuisine)
                .Member(h => h.WikiPediaInfo, m => m.wikipedia_info)
                .Member(h => h.AttractionTypes, m => m.attraction_types)


             .Member(h => h.ReviewRatingCount, m => m.review_rating_count)
              .Member(h => h.SubRatings, m => m.subratings)
              .Member(h => h.PhotoCount, m => m.photo_count)
              .Member(h => h.LocationDetail, m => m.location_string)
              .Member(h => h.TripTypes, m => m.trip_types)
              .Member(h => h.Reviews, m => m.reviews)
              .Member(h => h.Awards, m => m.awards);
        }
        public static bool IsRedisAvailable()
        {
            bool isRedisAvailable = false;
            RedisService redisService = new RedisService();
            isRedisAvailable = redisService.IsConnected();
            return isRedisAvailable;
        }

        public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
        {

            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    p.SetValue(dest, sourceProp.GetValue(source, null), null);
                }

            }

        }
    }
}
