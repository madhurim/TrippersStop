using ExpressMapper;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using TAVM = TraveLayer.CustomTypes.TripAdvisor.ViewModels;
using TGV = TraveLayer.CustomTypes.Google.ViewModels;
using TraveLayer.CustomTypes.Weather;
using TraveLayer.CustomTypes.Constants.Response;
using TraveLayer.CustomTypes.Constants.ViewModels;
using TraveLayer.CustomTypes.Google.Response;
using RS = TraveLayer.CustomTypes.TripAdvisor.Response;
using TraveLayer.CustomTypes.Yelp.Response;
using TraveLayer.CustomTypes.YouTube.Response;
using TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels;
using TraveLayer.CustomTypes.CurrencyConversion.ViewModels;
using TraveLayer.CustomTypes.CurrencyConversion.Response;
using TrippismApi.TraveLayer.Hotel.Sabre.HotelAvailabilityRequest;
using TraveLayer.CustomTypes.SabreSoap.ViewModels;

namespace Trippism.APIHelper
{
    public class RegisterMap
    {
        public void RegisterMappingEntities()
        {
            Mapper.Register<OTA_DestinationFinder, Fares>();
            Mapper.Register<OTA_FareRange, VM.FareRange>();
            Mapper.Register<FareData, VM.RecentlyPaid>();
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

            Mapper.Register<GoogleOutput, TGV.Google>();
            Mapper.Register<results, TGV.results>();
            Mapper.Register<Geometry, TGV.Geometry>();
            Mapper.Register<TraveLayer.CustomTypes.Google.Response.Location, TGV.Location>();

            Mapper.Register<BargainFinderReponse, LowFareSearch>()
                 .Member(h => h.AirLowFareSearchRS, m => m.OTA_AirLowFareSearchRS);

            Mapper.Register<OTA_AirportsAtCitiesLookup, AirportsAtCities>();
            Mapper.Register<OTA_AirportsAtCitiesLookup, CityPairs>();
            Mapper.Register<OTA_CountriesLookup, Countries>();
            Mapper.Register<OTA_MultiAirportCityLookup, MultiAirportCity>();
            Mapper.Register<OTA_PointofSaleCountryCodeLookup, PointofSaleCountryCode>();
            Mapper.Register<OTA_ThemeAirportLookup, ThemeAirport>();
            Mapper.Register<TopDestinations, TopDestination>();
            Mapper.Register<OTA_TravelThemeLookup, TravelTheme>();

            Mapper.Register<YelpOutput, TraveLayer.CustomTypes.Yelp.ViewModels.Yelp>();
            Mapper.Register<Businesses, TraveLayer.CustomTypes.Yelp.ViewModels.Businesses>();
            Mapper.Register<TraveLayer.CustomTypes.Yelp.Response.Location, TraveLayer.CustomTypes.Yelp.ViewModels.Location>();
            Mapper.Register<Region, TraveLayer.CustomTypes.Yelp.ViewModels.Region>();
            Mapper.Register<Span, TraveLayer.CustomTypes.Yelp.ViewModels.Span>();
            Mapper.Register<Center, TraveLayer.CustomTypes.Yelp.ViewModels.Center>();

            Mapper.Register<YouTubeOutput, TraveLayer.CustomTypes.YouTube.ViewModels.YouTube>();
            Mapper.Register<Items, TraveLayer.CustomTypes.YouTube.ViewModels.Items>();
            Mapper.Register<Snippet, TraveLayer.CustomTypes.YouTube.ViewModels.Snippet>();
            Mapper.Register<Id, TraveLayer.CustomTypes.YouTube.ViewModels.Id>();
            Mapper.Register<Thumbnails, TraveLayer.CustomTypes.YouTube.ViewModels.Thumbnails>();
            Mapper.Register<Default, TraveLayer.CustomTypes.YouTube.ViewModels.Default>();
            Mapper.Register<Medium, TraveLayer.CustomTypes.YouTube.ViewModels.Medium>();
            Mapper.Register<High, TraveLayer.CustomTypes.YouTube.ViewModels.High>();
            Mapper.Register<pageInfo, TraveLayer.CustomTypes.YouTube.ViewModels.pageInfo>();

            Mapper.Register<GooglePlaceDetailsOutput, TraveLayer.CustomTypes.Google.ViewModels.GooglePlaceDetails>();
            Mapper.Register<GooglePlaceresults, TraveLayer.CustomTypes.Google.ViewModels.GooglePlaceresults>();
            Mapper.Register<GooglePlaceGeometry, TraveLayer.CustomTypes.Google.ViewModels.GooglePlaceGeometry>();
            Mapper.Register<GooglePlaceLocation, TraveLayer.CustomTypes.Google.ViewModels.GooglePlaceLocation>();
            Mapper.Register<GooglePlacePhotos, TraveLayer.CustomTypes.Google.ViewModels.GooglePlacePhotos>();
            Mapper.Compile();
        }


        public void RegisterTripAdvisorMapping()
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

        public static void RegisterSabreSoapMapping()
        {

            Mapper.Register<OTA_HotelAvailRSAvailabilityOptionBasicPropertyInfoProperty, HotelRating>()
             .Member(h => h.Rating, m => m.Rating)
             .Member(h => h.RatingText, m => m.Text);

            Mapper.Register<OTA_HotelAvailRSAvailabilityOptionBasicPropertyInfoRateRange, RateRange>()
             .Member(h => h.CurrencyCode, m => m.CurrencyCode)
             .Member(h => h.Min, m => m.Min)
             .Member(h => h.Max, m => m.Max);

            Mapper.Register<OTA_HotelAvailRSAvailabilityOptionBasicPropertyInfo, HotelDetail>()
             .Member(h => h.Address, m => m.Address)
             .Member(h => h.HotelCityCode, m => m.HotelCityCode)
             .Member(h => h.HotelCode, m => m.HotelCode)
             .Member(h => h.HotelName, m => m.HotelName)
             .Member(h => h.Latitude, m => m.Latitude)
             .Member(h => h.HotelRating, m => m.Property)
             .Member(h => h.RateRange, m => m.RateRange)
             .Member(h => h.Longitude, m => m.Longitude);

            Mapper.Register<OTA_HotelAvailRSAvailabilityOptionBasicPropertyInfoPropertyOptionInfo, PropertyOptionInfo>()
                .Member(h => h.FreeWifiInRooms, m => m.FreeWifiInRooms.Ind);

            Mapper.Register<OTA_HotelAvailRSAvailabilityOption, HotelAvailability>()
                .Member(h => h.HotelDetail, m => m.BasicPropertyInfo);

            Mapper.Register<OTA_HotelAvailRS, Hotels>()
           .Member(h => h.HotelAvailability, m => m.AvailabilityOptions);

            Mapper.Register<OTA_HotelAvailRS, HotelInfo>()
              .Member(h => h.HotelAvailability, m => m.AvailabilityOptions);

        }
    }
}