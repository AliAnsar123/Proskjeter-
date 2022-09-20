using System;
using Xunit;
using Moq; // M? legge til pakken Moq.EntityFreamworkCore
using Gruppeoppgave_1.Controllers; // m? legge til en prosjektreferanse i Project-> Add Reference -> Project
using Gruppeoppgave_1.DAL;
using Gruppeoppgave_1.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using xUnitTestProject;
using System.Net;

namespace GruppeoppgaveTest
{

    public class RouteTimeTest
    {

        private readonly Mock<IRouteTimeRepository> mockRep = new Mock<IRouteTimeRepository>();
        private readonly Mock<ILogger<RouteTimeController>> mockLog = new Mock<ILogger<RouteTimeController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();
        private readonly RouteTime routeTimeTest = new RouteTime { Price = 9999, Date = new DateTime(2021, 11, 12), Direction = 0 };




        [Fact]
        public async Task HentAlle()
        {
            var routeTime1 = new RouteTime { Price = 9999, Date = new DateTime(2021, 11, 12), Direction = 0 };
            var routeTime2 = new RouteTime { Price = 9999, Date = new DateTime(2021, 11, 13), Direction = 1 };
            var routeTime3 = new RouteTime { Price = 9999, Date = new DateTime(2021, 11, 14), Direction = 2 };

            var routeTimeListe = new List<RouteTime>();
            routeTimeListe.Add(routeTime1);
            routeTimeListe.Add(routeTime2);
            routeTimeListe.Add(routeTime3);

            mockRep.Setup(k => k.GetAll()).ReturnsAsync(routeTimeListe);

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            var resultat = await routeTimeController.GetAll() as ObjectResult;

            // Assert 
            Assert.Equal<List<RouteTime>>((List<RouteTime>)resultat.Value, routeTimeListe);

        }

        [Fact]
        public async Task LagreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Create(routeTimeTest)).ReturnsAsync("Route time created");

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeTimeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeTimeController.Create(routeTimeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Route time created", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnIkkeOK()
        {
            // Arrange
            RouteTime routeTime1 = new RouteTime { Price = 9999, Date = new DateTime(2021, 11, 12), Direction = 1 }; 

            mockRep.Setup(k => k.Create(routeTime1)).ReturnsAsync("Name is required");

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeTimeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeTimeController.Create(routeTime1) as ObjectResult;

            // Assert 
            Assert.Equal("Name is required", resultat.Value);

        }

        [Fact]
        public async Task LagreLoggetInnFeilModel()
        {
            // Arrange

            RouteTime routeTime1 = new RouteTime { Price = 9999, Date = new DateTime(2021, 11, 12) }; // Den har ikke Direction

            mockRep.Setup(k => k.Create(routeTime1)).ReturnsAsync("Route time created");

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);


            routeTimeController.ModelState.AddModelError("Direction", "Feil i inputvalidering på server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeTimeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeTimeController.Create(routeTime1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task LagreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Create(routeTimeTest)).ReturnsAsync("Route time created");

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeTimeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeTimeController.Create(routeTimeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task SlettLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Route time with id: '" + 1 + "' deleted");

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeTimeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeTimeController.Delete(1) as ObjectResult;

            // Assert 
            Assert.Equal("Route time with id: '" + 1 + "' deleted", resultat.Value);

        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Route time with id: '" + 1 + "' deleted");

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeTimeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeTimeController.Delete(1) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }
        
        [Fact]
        public async Task HentEnOK()
        {
            // Arrange
            List<RouteTime> list = new List<RouteTime>();
            mockRep.Setup(k => k.GetByRouteId(1)).ReturnsAsync(list);

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await routeTimeController.GetByRouteId(1) as ObjectResult;

            // Assert 
            Assert.Equal<List<RouteTime>>(list, (List<RouteTime>)resultat.Value);
        }
        
        [Fact]
        public async Task HentEnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetByRouteId(1)).ReturnsAsync(() => null); 

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await routeTimeController.GetByRouteId(1) as ObjectResult;

            // Assert 
            Assert.Equal(null, resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(1,routeTimeTest)).ReturnsAsync("Route time with id: '" + 1 + "' updated");

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeTimeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeTimeController.Update(1, routeTimeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Route time with id: '" + 1 + "' updated", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(0, routeTimeTest)).ReturnsAsync("Route time with id: '" + 0 + "' not found");

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeTimeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeTimeController.Update(0, routeTimeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Route time with id: '" + 0 + "' not found", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
            // Arrange

            var routeTime1 = new RouteTime { Price = 9999, Date = new DateTime(2021, 11, 12)}; //Den har ikke Direction

            mockRep.Setup(k => k.Update(1,routeTime1)).ReturnsAsync("Route time with id: '" + 1 + "' updated");

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);


            routeTimeController.ModelState.AddModelError("Direction", "Feil i inputvalidering på server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeTimeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeTimeController.Update(1, routeTime1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Update(1, routeTimeTest)).ReturnsAsync("RouteTime with id: '" + 1 + "' updated");

            var routeTimeController = new RouteTimeController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeTimeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeTimeController.Update(1, routeTimeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }


    }
}
