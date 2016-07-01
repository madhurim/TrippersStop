using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using NSubstitute;
using TrippismProfiles;
using TrippismRepositories;
using TraveLayer;
using TrippismApi.TraveLayer;
using TrippismProfiles.Controllers;
using TrippismEntities;
using System.Collections.Generic;

namespace TrippismTests
{
    
    public class ActivityControllerTest
    {
       [Fact]
        public void ActivityGet()
        {
            var searchCriteria = Substitute.For<SearchCriteria>();
            searchCriteria.Destination = "NYC";
            List<SearchCriteria> sc = new List<SearchCriteria>();
            sc.Add(searchCriteria);

            var fakeRepository = Substitute.For<IActivityRepository>();  
            fakeRepository.FindSearch(Arg.Any<Guid>()).Returns(sc);
            var fakeService = Substitute.For<ICacheService>();

            ActivityController controllerToTest = new ActivityController(fakeService,fakeRepository);
            var result = controllerToTest.GetAllSearch(new Guid()).Result;

            Xunit.Assert.NotNull(sc);

        }
    }
}
