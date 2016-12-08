using System;
using Xunit;
using Xunit.Abstractions;
using NSubstitute;
using BusinessLogic;
using System.Collections.Generic;
using TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels;
using TraveLayer.CustomTypes.SabreSoap.ViewModels;
using TrippismApi;
using TrippismApi.TraveLayer.Hotel.Sabre;
using TrippismApi.TraveLayer;
using TraveLayer.CustomTypes.Sabre.SoapServices.Request;
using TrippismApi.TraveLayer.Hotel.Sabre.HotelAvailabilityRequest;
using TrippismApi.Areas.Sabre.Controllers;

namespace TrippismTests
{
 
    public class HotelBusinessLayerTests
    {
        [Fact]
        public void ProcessMethodReturnsNullonNull()
        {
            Hotels hotelList = new Hotels();
            HotelAvailability avail = new HotelAvailability();
            avail.HotelDetail = new HotelDetail();
            avail.HotelDetail.HotelRating = null;
            hotelList.HotelAvailability.Add(avail);
                       
            SabreHotelSoapCallerBusinessLayer businessLayer = new SabreHotelSoapCallerBusinessLayer();
            HotelOutput output = businessLayer.Process(hotelList);

            Assert.Null(output.ThreeStar);
            Assert.Null(output.FiveStar);
            Assert.Null(output.FourStar);
        }
        [Fact]
        public void Call_SoapService()
        {
            var subForBusinessLayer = Substitute.For<IBusinessLayer<Hotels, HotelOutput>>();
            var subForSabreSoapCaller = Substitute.For<ISabreHotelSoapCaller>();
            var subForCacheServices = Substitute.For<ICacheService>();

            
            HotelOutput ho = new HotelOutput();
            var output = subForBusinessLayer.Process(Arg.Any<Hotels>()).Returns<HotelOutput>(ho);

            string retfromCache = null;
            subForCacheServices.GetByKey<string>(Arg.Any<string>()).Returns<string>(retfromCache);

            //stub for returning any response given a request
            OTA_HotelAvailRS hotelRS = new OTA_HotelAvailRS();
            subForSabreSoapCaller.Received().GetHotels(Arg.Any<HotelRequest>()).Returns<OTA_HotelAvailRS>(hotelRS);
        }
        [Fact]
        public void IsBusinessLayerCalled()
        {            
            
            var subForSabreSoapCaller = Substitute.For<ISabreHotelSoapCaller>();           
            var subForCacheServices = Substitute.For<ICacheService>();
            var subForBusinessLayer = Substitute.For<IBusinessLayer<Hotels, HotelOutput>>();

            HotelController hotController = new HotelController(subForSabreSoapCaller, subForCacheServices, subForBusinessLayer);
            OTA_HotelAvailRS hotelRS = new OTA_HotelAvailRS();

            //string retfromCache = null;
            //subForCacheServices.GetByKey<string>(Arg.Any<string>()).Returns<string>(retfromCache);

            //Meant to test GetResponse , but it's not a public method.
            //hotController.

            Hotels hotelList = new Hotels();

            subForSabreSoapCaller.Received().GetHotels(Arg.Any<HotelRequest>()).Returns<OTA_HotelAvailRS>(hotelRS);
            subForBusinessLayer.Received().Process(hotelList);

        }
    }
}
