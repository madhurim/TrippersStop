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
using DataLayer;

namespace TrippismTests
{
   
    public class ActivityRepositoryTest
    {
        [Fact]
        public void FindSearchTest()
        {
          /*  var searchCriteria = Substitute.For<SearchCriteria>();
            searchCriteria.Destination = "NYC";
            List<SearchCriteria> sc = new List<SearchCriteria>();
            sc.Add(searchCriteria);

            var fakeDBContext = Substitute.For<IDBContext>();
            //this needs a lambda expression as an argument
            //foo.Bar(0, "").ReturnsForAnyArgs(x => "Hello " + x.Arg<string>());
            fakeDBContext.Find<List<SearchCriteria>>(Arg.Any<object>()).ReturnsForAnyArgs(sc);
            var fakeService = Substitute.For<ICacheService>();

            ActivityController controllerToTest = new ActivityController(fakeService, fakeRepository);
            var result = controllerToTest.GetAllSearch(new Guid()).Result;

            Xunit.Assert.NotNull(sc);*/
        }
    }
}
