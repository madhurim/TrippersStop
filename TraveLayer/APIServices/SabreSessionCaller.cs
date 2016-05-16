using System;
using System.Configuration;
using System.Net.Http;
using TrippismApi.TraveLayer.Hotel.Sabre.SessionCreateRequest;

namespace TrippismApi.TraveLayer.Hotel.Sabre
{
    public class SabreSessionCaller 
    {

        const string SessionAction = "SessionCreateRQ";
        const string SessionService = "SessionCreate";
        const string SessionPartyTo = "WebServiceSupplier";
        const string SessionPartyFrom = "WebServiceClient";

        private string SabreSoapBaseServiceURL
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSoapBaseServiceURL"];
            }
        }


        private string UserName
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSoapUserName"];
            }
        }
        private string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSoapPassword"];
            }
        }
        private string IPCC
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSoapIPCC"];
            }
        }
        private string Domain
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreSoapDomain"];
            }
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

        public string GetToken()
        {
            SessionCreateRS sessionCreateRS;
            try
            {
                DateTime dt = DateTime.UtcNow;
                string tstamp = dt.ToString("s") + "Z";

                MessageHeader msgHeader = GetMessageHeader();

                Security security = new Security()
                 {
                     UsernameToken = GetSecurityUserToken()
                 };

                SessionCreateRQPOSSource source = new SessionCreateRQPOSSource()
                    {
                        PseudoCityCode = this.IPCC
                    };

                SessionCreateRQPOS pos = new SessionCreateRQPOS()
                    {
                        Source = source
                    };

                SessionCreateRQ sessionCreateRQ = new SessionCreateRQ()
                    {
                        POS = pos
                    };

                SessionCreateRQService sessionCreateRQService = new SessionCreateRQService()
                {
                    Url=SabreSoapBaseServiceURL,
                    MessageHeaderValue = msgHeader,
                    SecurityValue = security
                };

                sessionCreateRS = sessionCreateRQService.SessionCreateRQ(sessionCreateRQ);

                if (sessionCreateRS.Errors != null && sessionCreateRS.Errors.Error != null)
                {
                    throw new HttpRequestException(string.Format("Sabre request failed: {0}", sessionCreateRS.Errors.Error.ErrorInfo.Message));
                }
                msgHeader = sessionCreateRQService.MessageHeaderValue;
                security = sessionCreateRQService.SecurityValue;
                return security.BinarySecurityToken;

            }
            catch (Exception ex)
            {
                throw new HttpRequestException(string.Format("Sabre request Exception :  {0}", ex.Message));
            }
         
        }

        private MessageHeader GetMessageHeader()
        {
            return new MessageHeader()
            {
                ConversationId = "HotelRequest",
                From = this.GetFromMessageHeader(),
                To = this.GetToMessageHeader(),
                CPAId = this.IPCC,
                Action =SessionAction ,
                Service = GetService(SessionService),
                MessageData = GetMessageData()
            };
        }

        private Service GetService(string name)
        {
            Service service = new Service()
            {
                Value = name
            };
            return service;
        }

        private To GetToMessageHeader()
        {
            To to = new To();
            PartyId toPartyId = new PartyId();
            PartyId[] toPartyIdArr = new PartyId[1];
            toPartyId.Value = SessionPartyTo;
            toPartyIdArr[0] = toPartyId;
            to.PartyId = toPartyIdArr;
            return to;
        }

        private From GetFromMessageHeader()
        {
            From from = new From();
            PartyId fromPartyId = new PartyId();
            PartyId[] fromPartyIdArr = new PartyId[1];
            fromPartyId.Value = SessionPartyFrom;
            fromPartyIdArr[0] = fromPartyId;
            from.PartyId = fromPartyIdArr;
            return from;
        }

        /// <summary>
        ///  Set user information, including security credentials and the IPCC.
        /// </summary>
        /// <returns></returns>
        private SecurityUsernameToken GetSecurityUserToken()
        {
            return new SecurityUsernameToken()
            {
                Username = this.UserName,
                Password = this.Password,
                Organization = this.IPCC,
                Domain = this.Domain
            };
        }

     
    }
}
