using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.NLog;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Request;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using Trippism.APIExtention.Filters;
using Trippism.APIHelper;
using TrippismApi;
using TrippismApi.Areas.Sabre.Controllers;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.Sabre.Controllers
{
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class InstaFlightSearchController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        private const string _nLoggerName = "InstaFlightLogger";
        public string SabreInstaFlightUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreInstaFlightUrl"];
            }
        }

        public string SabreCountriesUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSaleCountryUrl"];
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
            { return GetResponse(url, instaflightRequest); });
        }

        [Route("api/instaflight/GetDestination")]
        public async Task<HttpResponseMessage> GetDestination([FromUri]Destinations destinationsRequest)
        {
            return await Task.Run(() =>
            { return GetDestinationResponse(destinationsRequest); });
        }

        private HttpResponseMessage GetResponse(string url, InstaFlightSearchInput instaflightRequest)
        {
            APIResponse result = GetAPIResponse(url);
            string dateFormat = "yyyy-MM-dd";
            if (result.StatusCode == HttpStatusCode.NotFound && instaflightRequest.inboundflightstops != null && instaflightRequest.outboundflightstops != null)
            {
                InstaFlightNLog instaFlightNLog = new InstaFlightNLog { Request = result.RequestUrl, Response = result.OriginalResponse };
                TrippismNLog.SaveNLogData(instaFlightNLog.ToJson(), _nLoggerName);
                instaflightRequest.inboundflightstops = null;
                instaflightRequest.outboundflightstops = null;
                url = GetURL(instaflightRequest);
                result = GetAPIResponse(url);
            }

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var instaFlight = GetInstaFlightSearchData(result);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, instaFlight);
                return response;
            }
            else if (result.StatusCode == HttpStatusCode.NotFound)
            {
                InstaFlightNLog instaFlightNLog = new InstaFlightNLog { Request = result.RequestUrl, Response = result.OriginalResponse };
                TrippismNLog.SaveNLogData(instaFlightNLog.ToJson(), _nLoggerName);
                if (instaflightRequest.DepartureDate == DateTime.Now.ToString(dateFormat))
                {
                    // Getting insta flight data for next day after departure date.                
                    DateTime departureDate = DateTime.ParseExact(instaflightRequest.DepartureDate, dateFormat, null);
                    DateTime returnDate = DateTime.ParseExact(instaflightRequest.ReturnDate, dateFormat, null);
                    instaflightRequest.DepartureDate = departureDate.AddDays(1).ToString(dateFormat);
                    instaflightRequest.ReturnDate = returnDate.AddDays(1).ToString(dateFormat);
                    url = GetURL(instaflightRequest);
                    result = GetAPIResponse(url);
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var instaFlight = GetInstaFlightSearchData(result);
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, instaFlight);
                        return response;
                    }
                    else if (result.StatusCode == HttpStatusCode.NotFound)
                    {
                        instaFlightNLog = new InstaFlightNLog { Request = result.RequestUrl, Response = result.OriginalResponse };
                        TrippismNLog.SaveNLogData(instaFlightNLog.ToJson(), _nLoggerName);
                        // Getting insta flight data for 2 days after departure date.
                        instaflightRequest.DepartureDate = departureDate.AddDays(2).ToString(dateFormat);
                        instaflightRequest.ReturnDate = returnDate.AddDays(2).ToString(dateFormat);
                        url = GetURL(instaflightRequest);
                        result = GetAPIResponse(url);
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            var instaFlight = GetInstaFlightSearchData(result);
                            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, instaFlight);
                            return response;
                        }
                    }
                }
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }

        private InstaFlightSearch GetInstaFlightSearchData(APIResponse result)
        {
            var instaFlight = JsonObject.Parse(result.Response)
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
            return instaFlight;
        }
        private HttpResponseMessage GetDestinationResponse(Destinations destinationsRequest)
        {
            string urlNonStop = GetDestinationUrl(destinationsRequest, true);
            string urlMultiStop = GetDestinationUrl(destinationsRequest);
            APIResponse resultNonStop = GetAPIResponse(urlNonStop);
            APIResponse resultStop = GetAPIResponse(urlMultiStop);
            string posResponse = "Parameter 'pointofsalecountry' has an unsupported value";
            if (resultNonStop.StatusCode == HttpStatusCode.BadRequest
                && resultNonStop.Response.ToString() == posResponse 
                && resultStop.StatusCode == HttpStatusCode.BadRequest 
                && resultStop.Response.ToString() == posResponse)
            {
                APIResponse supportedPOSCountries = GetAPIResponse(SabreCountriesUrl);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, supportedPOSCountries.Response);
                return response;
            }
            else
            {
                Fares fares = new Fares();
                FareInfo fareInfo = new FareInfo();
                if (resultNonStop.StatusCode == HttpStatusCode.OK)
                {
                    var instaFlightNonStop = GetInstaFlightOutput(resultNonStop.Response);
                    if (instaFlightNonStop != null)
                    {
                        fareInfo.LowestNonStopFare = new LowestNonStopFare();
                        var pricedItineraries = instaFlightNonStop.PricedItineraries[0];
                        fareInfo.LowestNonStopFare.AirlineCodes = pricedItineraries.OriginDestinationOption[0]
                            .FlightSegment.Select(x => x.OperatingAirline.Code).Distinct().ToList();
                        fareInfo.LowestNonStopFare.Fare = pricedItineraries.AirItineraryPricingInfo[0].TotalFare.Amount;

                    fareInfo.CurrencyCode = pricedItineraries.AirItineraryPricingInfo[0].TotalFare.CurrencyCode;
                    fareInfo.DepartureDateTime = instaFlightNonStop.DepartureDateTime;
                    fareInfo.ReturnDateTime = instaFlightNonStop.ReturnDateTime;
                    fareInfo.DestinationLocation = instaFlightNonStop.DestinationLocation;
                    fares.OriginLocation = instaFlightNonStop.OriginLocation;
                }
            }
            else if (resultNonStop.StatusCode == HttpStatusCode.NotFound)
            {
                InstaFlightNLog instaFlightNLog = new InstaFlightNLog { Request = resultNonStop.RequestUrl, Response = resultNonStop.OriginalResponse };
                TrippismNLog.SaveNLogData(instaFlightNLog.ToJson(), _nLoggerName);
            }

            if (resultStop.StatusCode == HttpStatusCode.OK)
            {
                var instaFlightStop = GetInstaFlightOutput(resultStop.Response);
                if (instaFlightStop != null)
                {
                    fareInfo.LowestFare = new LowestFare();
                    var pricedItineraries = instaFlightStop.PricedItineraries[0];
                    fareInfo.LowestFare.AirlineCodes = pricedItineraries.OriginDestinationOption[0]
                        .FlightSegment.Select(x => x.OperatingAirline.Code).Distinct().ToList();
                    fareInfo.LowestFare.Fare = pricedItineraries.AirItineraryPricingInfo[0].TotalFare.Amount;

                    fareInfo.CurrencyCode = pricedItineraries.AirItineraryPricingInfo[0].TotalFare.CurrencyCode;
                    fareInfo.DepartureDateTime = instaFlightStop.DepartureDateTime;
                    fareInfo.ReturnDateTime = instaFlightStop.ReturnDateTime;
                    fareInfo.DestinationLocation = instaFlightStop.DestinationLocation;
                    fares.OriginLocation = instaFlightStop.OriginLocation;
                }
            }
            else if (resultStop.StatusCode == HttpStatusCode.NotFound)
            {
                InstaFlightNLog instaFlightNLog = new InstaFlightNLog { Request = resultStop.RequestUrl, Response = resultStop.OriginalResponse };
                TrippismNLog.SaveNLogData(instaFlightNLog.ToJson(), _nLoggerName);
            }
            fares.FareInfo = new List<FareInfo>();
            fares.FareInfo.Add(fareInfo);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fares);
            return response;
        }
        private InstaFlightSearch GetInstaFlightOutput(string response)
        {
            var result = JsonObject.Parse(response)
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
                                                           FlightSegment = originDestOpt.ArrayObjects("FlightSegment").ConvertAll<FlightSegmentViewModel>(flightSeg => new FlightSegmentViewModel()
                                                           {
                                                               OperatingAirline = flightSeg.Get<OperatingAirline>("OperatingAirline"),
                                                           })
                                                       }),
                        AirItineraryPricingInfo = pricedItinerary.ArrayObjects("AirItineraryPricingInfo")
                        .ConvertAll<AirItineraryPricingInfoViewModel>(airItineraryPricingInfo => new AirItineraryPricingInfoViewModel()
                        {
                            TotalFare = airItineraryPricingInfo.Object("ItinTotalFare").Object("TotalFare").ConvertTo<TotalFare>(totalFare => new TotalFare()
                            {
                                Amount = totalFare.Get<double>("Amount"),
                                DecimalPlaces = totalFare.Get<int>("DecimalPlaces"),
                                CurrencyCode = totalFare.Get<string>("CurrencyCode")
                            })
                        })
                    })
                });
            return result;
        }
        private APIResponse GetAPIResponse(string url)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            return result;
        }
        private string GetURL(InstaFlightSearchInput instaflightRequest)
        {
            StringBuilder url = new StringBuilder();
            url.Append(SabreInstaFlightUrl + "?");
            if (!string.IsNullOrWhiteSpace(instaflightRequest.Origin))
                url.Append("origin=" + instaflightRequest.Origin);
            if (!string.IsNullOrWhiteSpace(instaflightRequest.Destination))
                url.Append("&destination=" + instaflightRequest.Destination);
            if (!string.IsNullOrWhiteSpace(instaflightRequest.DepartureDate))
                url.Append("&departuredate=" + instaflightRequest.DepartureDate);
            if (!string.IsNullOrWhiteSpace(instaflightRequest.ReturnDate))
                url.Append("&returndate=" + instaflightRequest.ReturnDate);
            if (!string.IsNullOrWhiteSpace(instaflightRequest.IncludedCarriers))
                url.Append("&includedcarriers=" + instaflightRequest.IncludedCarriers);
            if (!string.IsNullOrWhiteSpace(instaflightRequest.PointOfSaleCountry))
                url.Append("&pointofsalecountry=" + instaflightRequest.PointOfSaleCountry);
            if (instaflightRequest.outboundflightstops != null)
                url.Append("&outboundflightstops=" + instaflightRequest.outboundflightstops);
            if (instaflightRequest.inboundflightstops != null)
                url.Append("&inboundflightstops=" + instaflightRequest.inboundflightstops);
            return url.ToString();
        }
        private string GetDestinationUrl(Destinations destinationsRequest, bool isNonStop = false)
        {
            StringBuilder url = new StringBuilder();
            url.Append(SabreInstaFlightUrl + "?");
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Origin))
                url.Append("origin=" + destinationsRequest.Origin);
            if (!string.IsNullOrWhiteSpace(destinationsRequest.Destination))
                url.Append("&destination=" + destinationsRequest.Destination);
            if (!string.IsNullOrWhiteSpace(destinationsRequest.DepartureDate))
                url.Append("&departuredate=" + destinationsRequest.DepartureDate);
            if (!string.IsNullOrWhiteSpace(destinationsRequest.ReturnDate))
                url.Append("&returndate=" + destinationsRequest.ReturnDate);
            if (!string.IsNullOrWhiteSpace(destinationsRequest.PointOfSaleCountry))
                url.Append("&pointofsalecountry=" + destinationsRequest.PointOfSaleCountry);
            if (isNonStop == true)
                url.Append("&outboundflightstops=0&inboundflightstops=0");
            url.Append("&limit=1");
            return url.ToString();
        }
    }
}
