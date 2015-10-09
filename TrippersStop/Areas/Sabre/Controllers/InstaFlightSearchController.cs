using System.Net;
using System.Net.Http;
using System.Web.Http;
using Trippism.APIExtention.Filters;
using TraveLayer.CustomTypes.Sabre;
using TrippismApi.TraveLayer;
using System.Configuration;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.Request;
using System.Text;
using TrippismApi;
using TraveLayer.CustomTypes.Sabre.Response;
using ServiceStack.Text;
using System;

namespace Trippism.Areas.Sabre.Controllers
{
    [GZipCompressionFilter]
    public class InstaFlightSearchController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;

        public string SabreInstaFlightUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreInstaFlightUrl"];
            }
        }

        public InstaFlightSearchController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }

        [Route("api/instaflight/search")]
        [Route("api/sabre/instaflight/search")]
        [ResponseType(typeof(InstaFlightSearch))]
        public async Task<HttpResponseMessage> Get([FromUri]InstaFlightSearchInput instaflightRequest)
        {
            string url = GetURL(instaflightRequest);
            return await Task.Run(() =>
            { return GetResponse(url); });
        }

        private string GetURL(InstaFlightSearchInput instaflightRequest)
        {
            StringBuilder url = new StringBuilder();
            url.Append(SabreInstaFlightUrl + "?");
            if (!string.IsNullOrWhiteSpace(instaflightRequest.Origin))
            {
                url.Append("origin=" + instaflightRequest.Origin);
            }
            if (!string.IsNullOrWhiteSpace(instaflightRequest.Destination))
            {
                url.Append("&destination=" + instaflightRequest.Destination);
            }
            if (!string.IsNullOrWhiteSpace(instaflightRequest.DepartureDate))
            {
                url.Append("&departuredate=" + instaflightRequest.DepartureDate);
            }
            if (!string.IsNullOrWhiteSpace(instaflightRequest.ReturnDate))
            {
                url.Append("&returndate=" + instaflightRequest.ReturnDate);
            }
            if (!string.IsNullOrWhiteSpace(instaflightRequest.IncludedCarriers))
            {
                url.Append("&includedcarriers=" + instaflightRequest.IncludedCarriers);
            }
            return url.ToString();
        }

        private HttpResponseMessage GetResponse(string url, int count = 0)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);            
            APIResponse result = _apiCaller.Get(url).Result;                        
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                InstaFlightsSearchOutput flights = new InstaFlightsSearchOutput();
                var fares = JsonObject.Parse(result.Response)
                    .ConvertTo(instaFlightSearch => new InstaFlightSearch
                {
                    DepartureDateTime = instaFlightSearch.Get<string>("DepartureDateTime"),
                    ReturnDateTime = instaFlightSearch.Get<string>("ReturnDateTime"),
                    DestinationLocation = instaFlightSearch.Get<string>("DestinationLocation"),
                    OriginLocation = instaFlightSearch.Get<string>("OriginLocation"),
                    PricedItineraries = instaFlightSearch.ArrayObjects("PricedItineraries").ConvertAll<PricedItineraryViewModel>(pricedItinerary => new PricedItineraryViewModel()
                    {
                        OriginDestinationOption = pricedItinerary.Object("AirItinerary").Object("OriginDestinationOptions").ArrayObjects("OriginDestinationOption")
                                                    .ConvertAll<OriginDestinationOptionViewModel>(originDestOpt => new OriginDestinationOptionViewModel()
                                                    {
                                                        #region OriginDestinationOption
                                                        ElapsedTime = originDestOpt.Get<int>("ElapsedTime"),
                                                        FlightSegment = originDestOpt.ArrayObjects("FlightSegment").ConvertAll<FlightSegmentViewModel>(flightSeg => new FlightSegmentViewModel()
                                                        {
                                                            DepartureAirport = flightSeg.Get<DepartureAirport>("DepartureAirport"),
                                                            ArrivalAirport = flightSeg.Get<ArrivalAirport>("ArrivalAirport"),
                                                            OperatingAirline = flightSeg.Get<OperatingAirline>("OperatingAirline"),
                                                            //DepartureTimeZone = flightSeg.Get<DepartureTimeZone>("DepartureTimeZone"),
                                                            // ArrivalTimeZone = flightSeg.Get<ArrivalTimeZone>("ArrivalTimeZone"),
                                                            DepartureDateTime = flightSeg.Get<string>("DepartureDateTime"),
                                                            ArrivalDateTime = flightSeg.Get<string>("ArrivalDateTime"),
                                                            StopQuantity = flightSeg.Get<int>("StopQuantity"),
                                                            FlightNumber = flightSeg.Get<string>("FlightNumber"),
                                                            ElapsedTime = flightSeg.Get<int>("ElapsedTime"),
                                                            StopAirport = flightSeg.Object("StopAirports") == null ? null : flightSeg.Object("StopAirports").ArrayObjects("StopAirport").ConvertAll<StopAirport>(stopAirport => new StopAirport()
                                                            {
                                                                DepartureDateTime = stopAirport.Get<string>("DepartureDateTime"),
                                                                ArrivalDateTime = stopAirport.Get<string>("ArrivalDateTime"),
                                                                LocationCode = stopAirport.Get<string>("LocationCode"),
                                                                ElapsedTime = stopAirport.Get<int>("ElapsedTime"),
                                                                Duration = stopAirport.Get<int>("Duration"),
                                                            })
                                                        })
                                                        #endregion
                                                    }),
                        #region AirItineraryPricingInfo
                        AirItineraryPricingInfo = pricedItinerary.ArrayObjects("AirItineraryPricingInfo")
                        .ConvertAll<AirItineraryPricingInfoViewModel>(airItineraryPricingInfo => new AirItineraryPricingInfoViewModel()
                        {
                            TotalFare = airItineraryPricingInfo.Object("ItinTotalFare").Object("TotalFare").ConvertTo<TotalFare>(totalFare => new TotalFare()
                            {
                                Amount = totalFare.Get<double>("Amount"),
                                DecimalPlaces = totalFare.Get<int>("DecimalPlaces"),
                                CurrencyCode = totalFare.Get<string>("CurrencyCode")
                            }),
                            #region COMMENTED
                            //PTC_FareBreakdown = airItineraryPricingInfo.Object("PTC_FareBreakdowns").ArrayObjects("PTC_FareBreakdown").ConvertAll<PTCFareBreakdownViewModel>(ptcFareBreakdown => new PTCFareBreakdownViewModel()
                            //{
                            //    #region PTC_FareBreakdown                          
                            //    PassengerTypeQuantity = ptcFareBreakdown.Object("PassengerTypeQuantity").ConvertTo<PassengerTypeQuantity>(passengerTypeQuant => new PassengerTypeQuantity()
                            //    {
                            //        Code = passengerTypeQuant.Get<string>("Code"),
                            //        Quantity = passengerTypeQuant.Get<int>("Quantity")
                            //    }),
                            //    PassengerFare = ptcFareBreakdown.Object("PassengerFare").ConvertTo<PassengerFare>(passengerFare => new PassengerFare()
                            //    {
                            //        #region PassengerFare
                            //        BaseFare = passengerFare.Get<BaseFare2>("BaseFare"),
                            //        FareConstruction = passengerFare.Get<FareConstruction2>("FareConstruction"),
                            //        EquivFare = passengerFare.Get<EquivFare2>("EquivFare"),
                            //        Taxes = ptcFareBreakdown.Object("Taxes").ConvertTo<Taxes2>(taxes => new Taxes2() { }),
                            //        TotalFare = passengerFare.Get<TotalFare2>("TotalFare"),
                            //        TPA_Extensions = ptcFareBreakdown.Object("TPA_Extensions").ConvertTo<TPAExtensions2>(TPAExt => new TPAExtensions2() { })
                            //        #endregion
                            //    })
                            //    #endregion
                            //}),
                            //FareInfo = airItineraryPricingInfo.ArrayObjects("FareInfo").ConvertAll<FareInfoViewModel>(fareInfo => new FareInfoViewModel()
                            //{
                            //    #region FareInfo
                            //    LowestFare = fareInfo.Get<LowestFare>("LowestFare"),
                            //    CurrencyCode = fareInfo.Get<string>("CurrencyCode"),
                            //    LowestNonStopFare = fareInfo.Get<LowestNonStopFare>("LowestNonStopFare"),
                            //    DepartureDateTime = fareInfo.Get<string>("DepartureDateTime"),
                            //    ReturnDateTime = fareInfo.Get<string>("ReturnDateTime"),
                            //    //public List<Link> Links { get; set; }
                            //    DestinationLocation = fareInfo.Get<string>("DestinationLocation"),
                            //    FareReference = fareInfo.Get<string>("FareReference"),
                            //    TPA_Extensions = fareInfo.Get<TPAExtensions4>("TPA_Extensions"),
                            //    #endregion
                            //})
                            #endregion
                        })
                        #endregion
                    })
                });

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fares);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
