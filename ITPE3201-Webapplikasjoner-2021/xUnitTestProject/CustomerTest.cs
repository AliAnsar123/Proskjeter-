using System;
using Xunit;
using Moq;  //M? legge til pakken Moq.EntityFreamworkCore
using Gruppeoppgave_1.Controllers;  //m? legge til en prosjektreferanse i Project-> Add Reference -> Project
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

    public class CustomerTest
    {

        private readonly Mock<ICustomerRepository> mockRep = new Mock<ICustomerRepository>();
        private readonly Mock<ILogger<CustomerController>> mockLog = new Mock<ILogger<CustomerController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();
        private readonly Customer customerTest = new Customer
        {
            FirstName = "Ola",
            LastName = "Arne",
            Email = "test1@email.com",
            Phone = 90213131,
            Street = "Olsoveien 1",
            ZipCode = new ZipCode { Id = "0001", City = "Oslo" }
        };



        [Fact]
        public async Task HentAlleLoggetInn()
        {
            var customer1 = new Customer
            {
                FirstName = "Ola",
                LastName = "Arne",
                Email = "test1@email.com",
                Phone = 90213131,
                Street = "Olsoveien 1",
                ZipCode = new ZipCode { Id = "0001", City = "Oslo" }
            };
            var customer2 = new Customer
            {
                FirstName = "Ola",
                LastName = "Arnes",
                Email = "test2@email.com",
                Phone = 90213132,
                Street = "Olsoveien 2",
                ZipCode = new ZipCode { Id = "0001", City = "Oslo" }
            };
           
            var customer3 = new Customer
            {
                FirstName = "Olass",
                LastName = "Arne",
                Email = "test3@email.com",
                Phone = 90213931,
                Street = "Olsoveien 3",
                ZipCode = new ZipCode { Id = "0001", City = "Oslo" }
            };

            var customerListe = new List<Customer>();
            customerListe.Add(customer1);
            customerListe.Add(customer2);
            customerListe.Add(customer3);

            mockRep.Setup(k => k.GetAll()).ReturnsAsync(customerListe);

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await customerController.GetAll() as ObjectResult;

            // Assert 
            Assert.Equal<List<Customer>>((List<Customer>)resultat.Value, customerListe);

        }
        [Fact]
        public async Task HentAlleIkkeLoggetInn()
        {
            var customer1 = new Customer
            {
                FirstName = "Ola",
                LastName = "Arne",
                Email = "test1@email.com",
                Phone = 90213131,
                Street = "Olsoveien 1",
                ZipCode = new ZipCode { Id = "0001", City = "Oslo" }
            };
            var customer2 = new Customer
            {
                FirstName = "Ola",
                LastName = "Arnes",
                Email = "test2@email.com",
                Phone = 90213132,
                Street = "Olsoveien 2",
                ZipCode = new ZipCode { Id = "0001", City = "Oslo" }
            };

            var customer3 = new Customer
            {
                FirstName = "Olass",
                LastName = "Arne",
                Email = "test3@email.com",
                Phone = 90213931,
                Street = "Olsoveien 3",
                ZipCode = new ZipCode { Id = "0001", City = "Oslo" }
            };

            var customerListe = new List<Customer>();
            customerListe.Add(customer1);
            customerListe.Add(customer2);
            customerListe.Add(customer3);

            mockRep.Setup(k => k.GetAll()).ReturnsAsync(customerListe);

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await customerController.GetAll() as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);

        }


        [Fact]
        public async Task LagreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Create(customerTest)).ReturnsAsync("Customer with email: '" + customerTest.Email + "' created");

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await customerController.Create(customerTest) as ObjectResult;

            // Assert 
            Assert.Equal("Customer with email: '" + customerTest.Email + "' created", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnIkkeOK()
        {
             // Arrange
            var customer1 = new Customer { Email = "" };
            mockRep.Setup(k => k.Create(customer1)).ReturnsAsync("Email is required");

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
            var resultat = await customerController.Create(customer1) as ObjectResult;

             // Assert 
            // Assert.Equal("Email is required", resultat.Value);

        }

        [Fact]
        public async Task LagreLoggetInnFeilModel()
        {
             // Arrange
             // Customer er indikert feil med tomt Email her.

            var customer1 = new Customer { Email = "" };

            mockRep.Setup(k => k.Create(customer1)).ReturnsAsync("Customer with email: '" + customer1.Email + "' created");

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);


            customerController.ModelState.AddModelError("Email", "Feil i inputvalidering p? server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
            var resultat = await customerController.Create(customer1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering p? server", resultat.Value);
        }

        [Fact]
        public async Task LagreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Create(customerTest)).ReturnsAsync("Customer with email: '" + customerTest.Email + "' created");

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
            var resultat = await customerController.Create(customerTest) as ObjectResult;

             // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task SlettLoggetInnOK()
        {
             // Arrange

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Customer with id: '" + 1 + "' deleted");

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
            var resultat = await customerController.Delete(1) as ObjectResult;

             // Assert 
            Assert.Equal("Customer with id: '" + 1 + "' deleted", resultat.Value);

        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Customer with id: '" + 1 + "' deleted");

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
            var resultat = await customerController.Delete(1) as ObjectResult;

             // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task HentEnLoggetInnOK()
        {
             // Arrange

            mockRep.Setup(k => k.GetById(1)).ReturnsAsync(customerTest);

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;
            // Act
            var resultat = await customerController.GetById(1) as ObjectResult;

             // Assert 
            Assert.Equal<Customer>(customerTest, (Customer)resultat.Value);
        }

        [Fact]
        public async Task HentEnIkkeLoggetInn()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(1)).ReturnsAsync(customerTest);

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;
            // Act
            var resultat = await customerController.GetById(1) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task HentEnLoggetInnIkkeOK()
        {
             // Arrange

            mockRep.Setup(k => k.GetById(1)).ReturnsAsync(() => null);

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;
            // Act
            var resultat = await customerController.GetById(1) as ObjectResult;

             // Assert 
            Assert.Equal(null, resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnOK()
        {
             // Arrange

            mockRep.Setup(k => k.Update(1, customerTest)).ReturnsAsync("Customer with id: '" + 1 + "' updated");

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
            var resultat = await customerController.Update(1, customerTest) as ObjectResult;

             // Assert 
            Assert.Equal("Customer with id: '" + 1 + "' updated", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
             // Arrange

            mockRep.Setup(k => k.Update(0, customerTest)).ReturnsAsync("Customer with id: '" + 0 + "' not found");

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
            var resultat = await customerController.Update(0, customerTest) as ObjectResult;

             // Assert 
            Assert.Equal("Customer with id: '" + 0 + "' not found", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
             // Arrange
             // Customer er indikert feil med tomt Email her.

            var customer1 = new Customer { Email = "" };

            mockRep.Setup(k => k.Update(1, customer1)).ReturnsAsync("Customer with id: '" + 1 + "' updated");

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);


            customerController.ModelState.AddModelError("Email", "Feil i inputvalidering p? server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
            var resultat = await customerController.Update(1, customer1) as ObjectResult;

             // Assert 
            Assert.Equal("Feil i inputvalidering p? server", resultat.Value);
        }

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Update(1, customerTest)).ReturnsAsync("Customer with id: '" + 1 + "' updated");

            var customerController = new CustomerController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            customerController.ControllerContext.HttpContext = mockHttpContext.Object;

             // Act
            var resultat = await customerController.Update(1, customerTest) as ObjectResult;

             // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }


    }
}
