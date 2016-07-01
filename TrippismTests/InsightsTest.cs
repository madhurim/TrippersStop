using System;
using Xunit;
using Moq;
using TrippismApi.Areas.Sabre.Controllers;
using TrippismApi.TraveLayer;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Threading.Tasks;
using System.Web;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using System.Net.Http;
using System.Net;

namespace TrippismTests
{
    
    public class InsightsTest
    {
        private Mock<IAsyncSabreAPICaller> mockAPICaller;
        private Mock<IAsyncWeatherAPICaller> mockWeatherAPICaller;
        private Mock<ICacheService> mockCacheService;

        [Fact]
        public void TestFareInsightsResponse()
        {
            //Arrange
          /*  mockAPICaller = new Mock<IAsyncSabreAPICaller>();
            // mock this correctly
            APIResponse resp = new APIResponse();
            resp.StatusCode = System.Net.HttpStatusCode.OK;
            resp.Response = "{'FareRange':{'OriginLocation':'EWR','DestinationLocation':'DEL','FareData':[{'MaximumFare':2487.23,'MinimumFare':1206.53,'MedianFare':2453.53,'CurrencyCode':'USD','Count':'Low','DepartureDateTime':'2016-03-23T00:00:00','ReturnDateTime':'2016-03-29T00:00:00','Links':[{'rel':'shop','href':'https://api.test.sabre.com/v1/shop/flights?origin=EWR&destination=DEL&departuredate=2016-03-23&returndate=2016-03-29&pointofsalecountry=US'},{'rel':'forecast','href':'https://api.test.sabre.com/v1/forecast/flights/fares?origin=EWR&destination=DEL&departuredate=2016-03-23&returndate=2016-03-29'}],'OriginLocation':'EWR','DestinationLocation':'DEL'}]},'TravelSeasonality':{'DestinationLocation':'DEL','Seasonality':[{'YearWeekNumber':1,'SeasonalityIndicator':'High','WeekEndDate':'2017-01-08T00:00:00','NumberOfObservations':'GreaterThan10000','WeekStartDate':'2017-01-02T00:00:00'},{'YearWeekNumber':2,'SeasonalityIndicator':'High','WeekEndDate':'2017-01-15T00:00:00','NumberOfObservations':'GreaterThan10000','WeekStartDate':'2017-01-09T00:00:00'},{'YearWeekNumber':3,'SeasonalityIndicator':'High','WeekEndDate':'2017-01-22T00:00:00','NumberOfObservations':'GreaterThan10000','WeekStartDate':'2017-01-16T00:00:00'},{'YearWeekNumber':4,'SeasonalityIndicator':'High','WeekEndDate':'2017-01-29T00:00:00','NumberOfObservations':'GreaterThan10000','WeekStartDate':'2017-01-23T00:00:00'},{'YearWeekNumber':5,'SeasonalityIndicator':'High','WeekEndDate':'2017-02-05T00:00:00','NumberOfObservations':'GreaterThan10000','WeekStartDate':'2017-01-30T00:00:00'},{'YearWeekNumber':6,'SeasonalityIndicator':'High','WeekEndDate':'2017-02-12T00:00:00','NumberOfObservations':'GreaterThan10000','WeekStartDate':'2017-02-06T00:00:00'},{'YearWeekNumber':7,'SeasonalityIndicator':'High','WeekEndDate':'2017-02-19T00:00:00','NumberOfObservations':'GreaterThan10000','WeekStartDate':'2017-02-13T00:00:00'},{'YearWeekNumber':8,'SeasonalityIndicator':'Medium','WeekEndDate':'2017-02-26T00:00:00','NumberOfObservations':'GreaterThan10000','WeekStartDate':'2017-02-20T00:00:00'},{'YearWeekNumber':53,'SeasonalityIndicator':'Low','WeekEndDate':'2016-01-03T00:00:00','NumberOfObservations':'GreaterThan10000','WeekStartDate':'2015-12-28T00:00:00'}]}}";
            mockAPICaller.Setup<Task<APIResponse>>(c => c.Get(It.IsAny<string>())).Returns(Task.FromResult(resp));
            //Now mock the CacheService and Weather Service
            mockCacheService = new Mock<ICacheService>();
            mockWeatherAPICaller = new Mock<IAsyncWeatherAPICaller>();

            //Act
            InsightsController testController = new InsightsController(mockAPICaller.Object, mockWeatherAPICaller.Object,mockCacheService.Object);
            HttpResponseMessage outPut = testController.GetResponse(String.Empty, String.Empty,String.Empty,String.Empty);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, outPut.StatusCode);
            Assert.Equal("application/json", outPut.Content.Headers.ContentType.MediaType);*/
        }

      

    }
}
