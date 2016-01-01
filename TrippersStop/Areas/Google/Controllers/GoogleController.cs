﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Google.Request;
using TraveLayer.CustomTypes.Google.Response;
using TraveLayer.CustomTypes.Sabre.Response;
using TrippismApi.TraveLayer;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Trippism.Areas.GooglePlace.Controllers
{
    public class GooglePlaceController : ApiController
    {
        const string TrippismKey = "Trippism.GooglePlace.";
        IAsyncGoogleAPICaller _apiCaller;
        ICacheService _cacheService;

        public GooglePlaceController(IAsyncGoogleAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }

        [ResponseType(typeof(TraveLayer.CustomTypes.Google.ViewModels.Google))]
        [Route("api/googleplace/locationsearch")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri]GoogleInput locationsearch)
        {
            string cacheKey = TrippismKey + string.Join(".", locationsearch.Latitude, locationsearch.Longitude, locationsearch.Types, locationsearch.Keywords);
            var tripGooglePlace = _cacheService.GetByKey<TraveLayer.CustomTypes.Google.ViewModels.Google>(cacheKey);
            if (tripGooglePlace != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tripGooglePlace);
            }
            return await Task.Run(() =>
            { return GetResponse(locationsearch, cacheKey); });
        }

        private HttpResponseMessage GetResponse(GoogleInput locationsearch, string cacheKey)
        {
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";

           // &types=restaurant|cafe
            string[] types = locationsearch.Types.Replace("&types=", System.String.Empty).Split('|');

            //string strLatitudeandsLongitude = Latitude + "," + Longitude;
            string url = GetURL(locationsearch);
            //let's just get English language results
            url = url + "&language=en";
            APIResponse result = _apiCaller.Get(url).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                GoogleOutput googleplace = new GoogleOutput();
                googleplace = ServiceStackSerializer.DeSerialize<GoogleOutput>(result.Response);

                if (locationsearch.ExcludeTypes != null)
                    googleplace.results = googleplace.results.Where(x => !x.types.Intersect(locationsearch.ExcludeTypes).Any()).ToList();
                   

                //if next_page_token exists get the next set of results , combine , order by rating get top 20
                if (!string.IsNullOrWhiteSpace(googleplace.next_page_token))
                {
                    result = _apiCaller.Get(url + "&pagetoken=" + googleplace.next_page_token).Result;
                    GoogleOutput nextResult = ServiceStackSerializer.DeSerialize<GoogleOutput>(result.Response);
                    if (locationsearch.ExcludeTypes != null)
                        nextResult.results = nextResult.results.Where(x => !x.types.Intersect(locationsearch.ExcludeTypes).Any()).ToList();
                    googleplace.results.AddRange(nextResult.results);
                     
                    
                }

                //Filter : the types returned by Google should have the types in the request
                //Exclude lodging from restaurants
                //Keyword=Beach : the name in the result should have "beach" , "resort" or "resorts"

                //only if result is acquired with types. if the result is not acquired with types : for ex: Keyword=beach skip this logic
                if( types.Length > 0 && !types[0].Contains("keyword") )
                {
                    var tempcollection = new List<TraveLayer.CustomTypes.Google.Response.results>();

                    foreach (var output in googleplace.results)
                    {
                        bool matchfound = false;

                        foreach (string match in types)
                        {
                            if (output.types.Contains(match))
                            { matchfound = true; break; }
                        }
                        if (matchfound == false)
                        { tempcollection.Add(output); }

                    }
                    foreach (var output in tempcollection)
                    {
                        if (googleplace.results.Contains(output)) googleplace.results.Remove(output);
                    }
                
                }
                
               //Order by rank : The highest ranked first -
               googleplace.results = googleplace.results.OrderByDescending(x => x.rating).ToList();


                Mapper.CreateMap<GoogleOutput, TraveLayer.CustomTypes.Google.ViewModels.Google>();
                Mapper.CreateMap<results, TraveLayer.CustomTypes.Google.ViewModels.results>();
                Mapper.CreateMap<Geometry, TraveLayer.CustomTypes.Google.ViewModels.Geometry>();
                Mapper.CreateMap<Location, TraveLayer.CustomTypes.Google.ViewModels.Location>();
                //Mapper.CreateMap<Photos, TraveLayer.CustomTypes.Google.ViewModels.Photos>();

                TraveLayer.CustomTypes.Google.ViewModels.Google lstLocations = Mapper.Map<GoogleOutput, TraveLayer.CustomTypes.Google.ViewModels.Google>(googleplace);

                //is this a bug ? 
                if (string.IsNullOrWhiteSpace(locationsearch.NextPageToken))
                    _cacheService.Save<TraveLayer.CustomTypes.Google.ViewModels.Google>(cacheKey, lstLocations);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lstLocations);

                return response;
            }

            return Request.CreateResponse(result.StatusCode, result.Response);
        }
        private string GetURL(GoogleInput locationsearch)
        {
            string url = string.Empty;
            if (!string.IsNullOrWhiteSpace(locationsearch.NextPageToken))
                url += string.Format("&pagetoken={0}", locationsearch.NextPageToken);
            if (!string.IsNullOrWhiteSpace(locationsearch.Latitude) && !string.IsNullOrWhiteSpace(locationsearch.Longitude))
                url += string.Format("&location={0}", locationsearch.Latitude + "," + locationsearch.Longitude);
            if (!string.IsNullOrWhiteSpace(locationsearch.Types))
                url += locationsearch.Types;
            if (!string.IsNullOrWhiteSpace(locationsearch.Keywords))
            {
                url += locationsearch.Keywords;
                /* url += string.Format("&keyword={0}", locationsearch.Keywords);
               
                if (locationsearch.Keywords == "CARIBBEAN" || locationsearch.Keywords == "BEACH")
                    url += string.Format("&keyword={0}", locationsearch.Keywords + "|BEACH|BEACHES|WATERSPORTS|SCUBA|SNORKELING");
                else if (locationsearch.Keywords == "GAMBLING")
                {
                    url += string.Format("&keyword={0}", locationsearch.Keywords + "|GAMING|CASINO|CARDGAME");
                }
                else if (locationsearch.Keywords == "DISNEY")
                {
                    url += string.Format("&keyword={0}", locationsearch.Keywords + "|DISNEY|FUNPARK|MICKEYMOUSE|CARNIVAL|FUNFAIR|PLEASUREGROUND|SAFARIPARK|THEMEPARK|WATERPARK");
                }
                else if (locationsearch.Keywords == "HISTORIC")
                {
                    url += string.Format("&keyword={0}", locationsearch.Keywords + "|MONUMENTS|FORT|HERITAGE|PYRAMIDS|CAVES|PALACE");
                }
                else if (locationsearch.Keywords == "NATIONAL-PARKS" || locationsearch.Keywords == "THEME-PARK")
                {
                    url += string.Format("&keyword={0}", locationsearch.Keywords + "|FUNPARK|MICKEYMOUSE|CARNIVAL|FUNFAIR|PLEASUREGROUND|SAFARIPARK|THEMEPARK|WATERPARK");
                }
                else if (locationsearch.Keywords == "MOUNTAINS" )
                {
                    url += string.Format("&keyword={0}", locationsearch.Keywords + "|HILLSTATION|TREKKING|SKIING|WATERRAFTING|WATERSPORTS|RIVERSPORTS|SNOWSPORTS|HELISKIING");
                }
                else
                {
                    url += string.Format("&keyword={0}", locationsearch.Keywords);
                }
                 */
            }

            return url;
        }
    }
}