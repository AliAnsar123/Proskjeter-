
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

    public class OrderTest
    {

        private readonly Mock<IOrderRepository> mockRep = new Mock<IOrderRepository>();
        private readonly Mock<ILogger<OrderController>> mockLog = new Mock<ILogger<OrderController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();
        private readonly Order orderTest = new Order
        {
            DepartureRouteTime = new RouteTime { Price = 9999, Date = new DateTime(2021, 11, 12), Direction = 0 },
            ReturnRouteTime = new RouteTime { Price = 9999, Date = new DateTime(2021, 11, 14), Direction = 1 },
            NumberOfVehicles = 1,
            IsRoundTrip = true,
            MainCustomer = new Customer
            {
                FirstName = "Ola",
                LastName = "Arne",
                Email = "test1@email.com",
                Phone = 90213131,
                Street = "Olsoveien 1",
                ZipCode = new ZipCode { Id = "0001", City = "Oslo" }
            }
        };
        private readonly Order orderTest1 = new Order  // Order uten DepartureRouteTime for å få FEIL
        {
            ReturnRouteTime = new RouteTime { Price = 9999, Date = new DateTime(2021, 11, 14), Direction = 1 },
            NumberOfVehicles = 1,
            IsRoundTrip = true,
            MainCustomer = new Customer
            {
                FirstName = "Ola",
                LastName = "Arne",
                Email = "test1@email.com",
                Phone = 90213131,
                Street = "Olsoveien 1",
                ZipCode = new ZipCode { Id = "0001", City = "Oslo" }
            }
        };


        [Fact]
        public async Task HentAlle()
        {
          
            var orderListe = new List<Order>();
            orderListe.Add(orderTest);
            orderListe.Add(orderTest);
            orderListe.Add(orderTest);

            mockRep.Setup(k => k.GetAll()).ReturnsAsync(orderListe);

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await orderController.GetAll() as ObjectResult;

            // Assert 
            Assert.Equal<List<Order>>((List<Order>)resultat.Value, orderListe);

        }
        [Fact]
        public async Task HentAlleIkkeLoggetInn()
        {

            var orderListe = new List<Order>();
            orderListe.Add(orderTest);
            orderListe.Add(orderTest);
            orderListe.Add(orderTest);

            mockRep.Setup(k => k.GetAll()).ReturnsAsync(orderListe);

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await orderController.GetAll() as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);

        }

        [Fact]
        public async Task LagreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Create(orderTest)).ReturnsAsync("Order created");

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await orderController.Create(orderTest) as ObjectResult;

            // Assert 
            Assert.Equal("Order created", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnIkkeOK()
        {
            // Arrange
            mockRep.Setup(k => k.Create(orderTest1)).ReturnsAsync("NumberOfVehicles is required");

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await orderController.Create(orderTest1) as ObjectResult;

            // Assert 
            Assert.Equal("NumberOfVehicles is required", resultat.Value);

        }

        [Fact]
        public async Task LagreLoggetInnFeilModel()
        {
            // Arrange

            mockRep.Setup(k => k.Create(orderTest1)).ReturnsAsync("Order created");

            var orderController = new OrderController(mockRep.Object, mockLog.Object);


            orderController.ModelState.AddModelError("NumberOfVehicles", "Feil i inputvalidering p? server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await orderController.Create(orderTest1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering p? server", resultat.Value);
        }


        [Fact]
        public async Task SlettLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Order with id: '" + 1 + "' deleted");

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await orderController.Delete(1) as ObjectResult;

            // Assert 
            Assert.Equal("Order with id: '" + 1 + "' deleted", resultat.Value);

        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Order with id: '" + 1 + "' deleted");

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await orderController.Delete(1) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task HentEnOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(1)).ReturnsAsync(orderTest);

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;
            // Act
            var resultat = await orderController.GetById(1) as ObjectResult;

            // Assert 
            Assert.Equal<Order>(orderTest, (Order)resultat.Value);
        }

        [Fact]
        public async Task HentEnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(0)).ReturnsAsync(() => null);

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;
            // Act
            var resultat = await orderController.GetById(0) as ObjectResult;

            // Assert 
            Assert.Equal(null, resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(1, orderTest)).ReturnsAsync("Order with id: '" + 1 + "' updated");

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await orderController.Update(1, orderTest) as ObjectResult;

            // Assert 
            Assert.Equal("Order with id: '" + 1 + "' updated", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(0, orderTest)).ReturnsAsync("Order with id: '" + 0 + "' not found");

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await orderController.Update(0, orderTest) as ObjectResult;

            // Assert 
            Assert.Equal("Order with id: '" + 0 + "' not found", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
            // Arrange

            mockRep.Setup(k => k.Update(1, orderTest1)).ReturnsAsync("Order with id: '" + 1 + "' updated");

            var orderController = new OrderController(mockRep.Object, mockLog.Object);


            orderController.ModelState.AddModelError("NumberOfVehicles", "Feil i inputvalidering p? server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await orderController.Update(1, orderTest1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering p? server", resultat.Value);
        }

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Update(1, orderTest)).ReturnsAsync("Order with id: '" + 1 + "' updated");

            var orderController = new OrderController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            orderController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await orderController.Update(1, orderTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }


    }
}
