
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

    public class RouteTest
    {

        private readonly Mock<IRouteRepository> mockRep = new Mock<IRouteRepository>();
        private readonly Mock<ILogger<RouteController>> mockLog = new Mock<ILogger<RouteController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();
        private readonly Route routeTest = new Route {Origin = new Port { Id = 1, Name = "Oslo" },Destination = new Port { Id = 2, Name = "Kiel" },
                                                     Company = new Company { Id = 1, Name = "ColorLine" },};



        [Fact]
        public async Task HentAlle()
        {
            var route1 = new Route {Origin = new Port { Id = 1, Name = "Oslo" },Destination = new Port { Id = 2, Name = "Kiel" },
                                    Company = new Company { Id = 1, Name = "ColorLine" },};

            var route2 = new Route {Origin = new Port { Id = 1, Name = "Oslo" },Destination = new Port { Id = 3, Name = "Hirtsal" },
                                    Company = new Company { Id = 1, Name = "ColorLine" },};

            var route3 = new Route {Origin = new Port { Id = 1, Name = "Oslo" },Destination = new Port { Id = 4, Name = "Sverige" },
                                    Company = new Company { Id = 1, Name = "ColorLine" },};

            var routeListe = new List<Route>();
            routeListe.Add(route1);
            routeListe.Add(route2);
            routeListe.Add(route3);

            mockRep.Setup(k => k.GetAll()).ReturnsAsync(routeListe);

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            var resultat = await routeController.GetAll() as ObjectResult;

            // Assert 
            Assert.Equal<List<Route>>((List<Route>)resultat.Value, routeListe);

        }

        [Fact]
        public async Task LagreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Create(routeTest)).ReturnsAsync("Route created");

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeController.Create(routeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Route created", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnIkkeOK()
        {
            // Arrange
            // Route er indikert feil med tomt Origin her.
            var route1 = new Route
            {
                Destination = new Port { Id = 2, Name = "Kiel" },
                Company = new Company { Id = 1, Name = "ColorLine" },
            };
            mockRep.Setup(k => k.Create(route1)).ReturnsAsync("Origin is required");

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeController.Create(route1) as ObjectResult;

            // Assert 
            Assert.Equal("Origin is required", resultat.Value);

        }

        [Fact]
        public async Task LagreLoggetInnFeilModel()
        {
            // Arrange

            // Route er indikert feil med tomt Origin her.
            var route1 = new Route
            {
                Destination = new Port { Id = 2, Name = "Kiel" },
                Company = new Company { Id = 1, Name = "ColorLine" },
            };

            mockRep.Setup(k => k.Create(route1)).ReturnsAsync("Route created");

            var routeController = new RouteController(mockRep.Object, mockLog.Object);


            routeController.ModelState.AddModelError("Origin", "Feil i inputvalidering p? server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeController.Create(route1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering p? server", resultat.Value);
        }

        [Fact]
        public async Task LagreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Create(routeTest)).ReturnsAsync("Route created");

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeController.Create(routeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task SlettLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Route with id: '" + 1 + "' deleted");

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeController.Delete(1) as ObjectResult;

            // Assert 
            Assert.Equal("Route with id: '" + 1 + "' deleted", resultat.Value);

        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Route with id: '" + 1 + "' deleted");

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeController.Delete(1) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task HentEnOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(1)).ReturnsAsync(routeTest);

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await routeController.GetById(1) as ObjectResult;

            // Assert 
            Assert.Equal<Route>(routeTest, (Route)resultat.Value);
        }
        
        [Fact]
        public async Task HentEnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(1)).ReturnsAsync(() => null); 

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await routeController.GetById(1) as ObjectResult;

            // Assert 
            Assert.Equal(null, resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(1,routeTest)).ReturnsAsync("Route with id: '" + 1 + "' updated");

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeController.Update(1, routeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Route with id: '" + 1 + "' updated", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange
            // Route er indikert feil med tomt Origin her.

            mockRep.Setup(k => k.Update(0, routeTest)).ReturnsAsync("Route with id: '" + 0 + "' not found");

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeController.Update(0, routeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Route with id: '" + 0 + "' not found", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
            // Arrange
            // Route er indikert feil med tomt Origin her.
            var route1 = new Route
            {
                Destination = new Port { Id = 2, Name = "Kiel" },
                Company = new Company { Id = 1, Name = "ColorLine" },
            };

            mockRep.Setup(k => k.Update(1,route1)).ReturnsAsync("Route with id: '" + 1 + "' updated");

            var routeController = new RouteController(mockRep.Object, mockLog.Object);


            routeController.ModelState.AddModelError("Origin", "Feil i inputvalidering p? server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeController.Update(1, route1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering p? server", resultat.Value);
        }

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Update(1, routeTest)).ReturnsAsync("Route with id: '" + 1 + "' updated");

            var routeController = new RouteController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            routeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await routeController.Update(1, routeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }


    }
}
