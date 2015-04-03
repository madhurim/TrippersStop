using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippismApi.TraveLayer;
using ServiceStack;
using AutoMapper;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using System.Xml;
using System.Xml.Linq;
using System.Web.Hosting;
using TraveLayer.CustomTypes.Sabre.Response;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// Search for the lowest available priced itineraries based upon a travel date
    /// </summary>
    public class BargainFinderController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public BargainFinderController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;          
        }
        // POST api/bargainfinder
        /// <summary>
        /// To search for the lowest priced itineraries available for specific travel dates
        /// </summary>
        public HttpResponseMessage Post(BargainFinder bargainFinder)
        {
            var pos = GetPOS();
            if (bargainFinder != null && bargainFinder.OTA_AirLowFareSearchRQ!=null && pos != null)
            bargainFinder.OTA_AirLowFareSearchRQ.POS = pos;
            //TBD : URL configurable using XML
            SabreApiTokenHelper.SetApiToken(_apiCaller, _cacheService);
            
            APIResponse result = _apiCaller.Post("v1.8.2/shop/flights?mode=live", ServiceStackSerializer.Serialize(bargainFinder)).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                SabreApiTokenHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Post("v1.8.2/shop/flights?mode=live", ServiceStackSerializer.Serialize(bargainFinder)).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var bargainResponse = DeSerializeResponse(result.Response);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, bargainResponse);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response); 
        }
        /// <summary>
        /// Get Point of sale settings from config
        /// </summary>
        private POS GetPOS()
        {
            XElement xelement = XElement.Load(HostingEnvironment.MapPath("/Areas/Sabre/POS/PointOfSales.xml"));

            IEnumerable<XElement> ps = xelement.Elements("POS")
                   .Where(x => x.Attribute("IsActive").Value == "true" && x.Attribute("RequestType").Value == "BargainFinder"); 
            if (ps != null)
            {
                POS pos = new POS()
                {
                    Source = new List<Source>()
                };
                foreach(XElement p in ps)
                {
                    pos.Source.Add(new Source()
                    {
                        RequestorID = new RequestorID()
                        {
                            CompanyName = new CompanyName()
                            {
                                Code = p.Element("CompanyCode").Value
                            },
                            ID = p.Element("ID").Value,
                            Type = p.Element("Type").Value
                        }
                    });
                }
             
                return pos;
            }
            return null;
        }
        /// <summary>
        /// DeSerialize the Json request
        /// </summary>
        private LowFareSearch DeSerializeResponse(string result)
        {
            BargainFinderReponse reponse = new BargainFinderReponse();
            reponse = ServiceStackSerializer.DeSerialize<BargainFinderReponse>(result);
            Mapper.CreateMap<BargainFinderReponse, LowFareSearch>()
                    .ForMember(o => o.AirLowFareSearchRS, m => m.MapFrom(s => s.OTA_AirLowFareSearchRS));
            LowFareSearch lowFareSearch = Mapper.Map<BargainFinderReponse, LowFareSearch>(reponse);
            return lowFareSearch;
        }

    }

}
