﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrippismApi.TraveLayer;
using NSubstitute;
using TraveLayer.CustomTypes.Sabre.Response;
using TrippismApi.Areas.Sabre.Controllers;
using Trippism.TokenManagement;
using System.Net.Http;
using Xunit;

namespace TrippismTests
{
    public class TokenManagerTests
    {
        IAsyncSabreAPICaller apiCaller;
        ICacheService cacheService;
        ITokenManager tokenManager;
        ThemeAirportController controller;

        string jsonout = @"{
                            Destinations: [{
                                Destination: ""BOS"",
                                Type: ""Airport"",
                                Links: [{
                                    rel: ""shopTemplate"",
                                    href: ""https://api.sabre.com/v1/shop/flights/fares?origin=<origin>&departuredate=<departuredate>&returndate=<returndate>&location=<location>&theme=BEACH&minfare=<minfare>&maxfare=<maxfare>&lengthofstay=<lengthofstay>&earliestdeparturedate=<earliestdeparturedate>&latestdeparturedate=<latestdeparturedate>&pointofsalecountry=<pointofsalecountry>&region=<region>&topdestinations=<topdestinations>""
                                }]
                            }, 
                                {
                                Destination: ""CHS"",
                                Type: ""Airport"",
                                Links: [{
                                    rel: ""shopTemplate"",
                                    href: ""https://api.sabre.com/v1/shop/flights/fares?origin=<origin>&departuredate=<departuredate>&returndate=<returndate>&location=<location>&theme=BEACH&minfare=<minfare>&maxfare=<maxfare>&lengthofstay=<lengthofstay>&earliestdeparturedate=<earliestdeparturedate>&latestdeparturedate=<latestdeparturedate>&pointofsalecountry=<pointofsalecountry>&region=<region>&topdestinations=<topdestinations>""
                                }]
                        }],
                                Links: [{
                                rel: ""self"",
                                href: ""https://api.sabre.com/v1/lists/supported/shop/themes/BEACH""
                            }, {
                                rel: ""linkTemplate"",
                                href: ""https://api.sabre.com/v1/lists/supported/shop/themes/<theme>""
                            }]
                        }";

        public TokenManagerTests()
        {
            apiCaller = Substitute.For<IAsyncSabreAPICaller>();
            cacheService = Substitute.For<ICacheService>();
            tokenManager = Substitute.For<ITokenManager>();

            apiCaller.GetToken(Arg.Any<string>()).Returns<string>("DummyToken");
            cacheService.GetByKey<string>(Arg.Any<string>()).Returns<string>("DummyValue");
            apiCaller.LongTermToken = "DummyLongTermToken";
            apiCaller.TokenExpireIn = "DummyExpireIn";

            cacheService.Save<string>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>()).Returns<bool>(true);

            apiCaller.Authorization = "dummybearer";
            apiCaller.ContentType = "dummyapplication/json";
            apiCaller.ContentType = "dummyapplication/x-www-form-urlencoded";
            apiCaller.Get(Arg.Any<string>()).Returns<APIResponse>(new APIResponse
            {
                OriginalResponse = new System.Net.Http.HttpResponseMessage(),
                RequestUrl = "Url",
                StatusCode = System.Net.HttpStatusCode.OK,
                Response = jsonout
            });

            controller = new ThemeAirportController(apiCaller,cacheService,tokenManager);
        }
        [Fact]
        public void TestOKResponse()
        {          

            HttpResponseMessage actual = controller.GetResponse("http://url");

            Assert.NotNull(actual);

        }
    }
}

