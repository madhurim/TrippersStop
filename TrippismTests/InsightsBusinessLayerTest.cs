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
using BusinessLogic;

namespace TrippismTests
{
    public class InsightsBusinessLayerTest
    {
       
        [Fact]
        public void TestFareInsightsResponse()
        {
            var bobject = new FareInsightsBusinessLayer();
            FareOutput finput = new FareOutput();
            bobject.Process(finput);
        }
    }
}
