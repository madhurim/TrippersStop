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
            mockAPICaller = new Mock<IAsyncSabreAPICaller>();
            // mock this correctly
            APIResponse resp = new APIResponse();
            resp.StatusCode = System.Net.HttpStatusCode.OK;
            resp.Response = "The Response";
            mockAPICaller.Setup<Task<APIResponse>>(c => c.Get(It.IsAny<string>())).Returns(Task.FromResult(resp));
            //Now mock the CacheService and Weather Service
            mockCacheService = new Mock<ICacheService>();
            mockWeatherAPICaller = new Mock<IAsyncWeatherAPICaller>();

            //Act
            InsightsController testController = new InsightsController(mockAPICaller.Object, mockWeatherAPICaller.Object,mockCacheService.Object);
            HttpResponseMessage outPut = testController.GetFareResponse(String.Empty, String.Empty);
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, outPut.StatusCode);
            Assert.Equal("application/json", outPut.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public void TestFareForecastsResponse()
        {
            
            //Act
            InsightsController testController = new InsightsController(mockAPICaller.Object, mockWeatherAPICaller.Object, mockCacheService.Object);
            LowFareForecast outPut = testController.GetFareForecastResponse(String.Empty);

            //Assert
            Assert.Equal("US", outPut.CurrencyCode);

        }
    }
}
