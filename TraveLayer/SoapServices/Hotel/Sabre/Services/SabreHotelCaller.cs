using System;
using System.Configuration;
using System.IO;
using TraveLayer.CustomTypes.Sabre.SoapServices.Request;
using TraveLayer.SoapServices.Hotel;
using TraveLayer.SoapServices.Hotel.Sabre;
using TraveLayer.SoapServices.Hotel.Sabre.HotelAvailabilityRequest;

namespace TraveLayer.SoapServices.Hotel.Sabre
{
    public class SabreHotelCaller : ISabreHotel
    {
        const string HotelAvailabilityAction = "OTA_HotelAvailLLSRQ";
        const string HotelAvailabilityService = "HotelAvailLLSRQ";
        const string HotelAvailabilityPartyTo = "WebServiceSupplier";
        const string HotelAvailabilityPartyFrom = "WebServiceClient";
        public string SabreSessionTokenKey
        {
            get
            {
                return "TrippismApi.SabreSessionToken";
            }
        }

        private string IPCC
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSoapIPCC"];
            }
        }
        public string SecurityToken
        {
            get;
            set;
        }

        public OTA_HotelAvailRS GetHotels(HotelRequest hotelRequest)
        {
          
            MessageHeader msgHeader = new MessageHeader()
                {
                    ConversationId = string.Join("::", hotelRequest.HotelCityCode, hotelRequest.StartDate, hotelRequest.EndDate),
                    From = this.GetFromMessageHeader(),
                    To = this.GetToMessageHeader(),
                    CPAId = this.IPCC,
                    Action =HotelAvailabilityAction,
                    Service = GetService(HotelAvailabilityService),
                    MessageData = GetMessageData()
                };

            OTA_HotelAvailRQ hotelAvailRQ = new OTA_HotelAvailRQ();


            hotelAvailRQ.AvailRequestSegment = new OTA_HotelAvailRQAvailRequestSegment()
            {
                //Customer = GetCustomerDetails(),
                GuestCounts = GetGuestCount(hotelRequest),
                HotelSearchCriteria = GetHotelSearchCriteria(hotelRequest),
                TimeSpan = GetTimeSpan(hotelRequest),
                RatePlanCandidates = new OTA_HotelAvailRQAvailRequestSegmentRatePlanCandidates()
                {
                    RateRange = new OTA_HotelAvailRQAvailRequestSegmentRatePlanCandidatesRateRange()
                    {
                        CurrencyCode = hotelRequest.CurrencyCode,
                    }
                }
            };

            TraveLayer.SoapServices.Hotel.Sabre.HotelAvailabilityRequest.OTA_HotelAvailService serviceObj = new TraveLayer.SoapServices.Hotel.Sabre.HotelAvailabilityRequest.OTA_HotelAvailService();
            serviceObj.MessageHeaderValue = msgHeader;
            serviceObj.Security = new Security1()
                {
                    BinarySecurityToken = this.SecurityToken,
                };

            OTA_HotelAvailRS response = serviceObj.OTA_HotelAvailRQ(hotelAvailRQ);

            // if (resp..Errors != null && resp.Errors.Error != null)
            // {
            // }
            return response;
        }
        private To GetToMessageHeader()
        {
            To to = new To();
            PartyId toPartyId = new PartyId();
            PartyId[] toPartyIdArr = new PartyId[1];
            toPartyId.Value = HotelAvailabilityPartyTo;
            toPartyIdArr[0] = toPartyId;
            to.PartyId = toPartyIdArr;
            return to;
        }

        private From GetFromMessageHeader()
        {
            From from = new From();
            PartyId fromPartyId = new PartyId();
            PartyId[] fromPartyIdArr = new PartyId[1];
            fromPartyId.Value = HotelAvailabilityPartyFrom;
            fromPartyIdArr[0] = fromPartyId;
            from.PartyId = fromPartyIdArr;
            return from;
        }
        private OTA_HotelAvailRQAvailRequestSegmentTimeSpan GetTimeSpan(HotelRequest hotelRequest)
        {
            return new OTA_HotelAvailRQAvailRequestSegmentTimeSpan()
            {
                Start = hotelRequest.StartDate.ToString("MM-dd"),
                End = hotelRequest.EndDate.ToString("MM-dd")
            };
        }

        private OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteria GetHotelSearchCriteria(HotelRequest hotelRequest)
        {
            return new OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteria()
            {
                Criterion = new OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterion()
                {
                    HotelRef=GetHotelRef(hotelRequest),
                }
            };
        }

        private OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionHotelRef[] GetHotelRef(HotelRequest hotelRequest)
        {
            OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionHotelRef[] hotelref = new OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionHotelRef[1];
            hotelref[0] = new OTA_HotelAvailRQAvailRequestSegmentHotelSearchCriteriaCriterionHotelRef()
            {
                HotelCityCode = hotelRequest.HotelCityCode,
            };
            return hotelref;
        }

        private OTA_HotelAvailRQAvailRequestSegmentGuestCounts GetGuestCount(HotelRequest hotelRequest)
        {
            return new OTA_HotelAvailRQAvailRequestSegmentGuestCounts()
            {
                Count=hotelRequest.GuestCounts,
            };
        }

        private MessageData GetMessageData()
        {
            MessageData msgData = new MessageData();
            msgData.MessageId = "mid:20001209-133003-2333@clientofsabre.com1";
            msgData.Timestamp = GetTimeStamp();
            return msgData;
        }

        private string GetTimeStamp()
        {
            DateTime dt = DateTime.UtcNow;
            return dt.ToString("s") + "Z";
        }

        private Service GetService(string name)
        {
            Service service = new Service()
            {
                Value = name
            };
            return service;
        }

        private OTA_HotelAvailRQAvailRequestSegmentCustomer GetCustomerDetails()
        {
            return new OTA_HotelAvailRQAvailRequestSegmentCustomer()
            {
                Corporate = new OTA_HotelAvailRQAvailRequestSegmentCustomerCorporate()
                {
                    ID = string.Empty,
                }
            };
        }

    }
}
