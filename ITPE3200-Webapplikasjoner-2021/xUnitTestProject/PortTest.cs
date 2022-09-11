
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

    public class PortTest
    {

        private readonly Mock<IPortRepository> mockRep = new Mock<IPortRepository>();
        private readonly Mock<ILogger<PortController>> mockLog = new Mock<ILogger<PortController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();
        private readonly Port portTest = new Port { Name = "Stenaa" };



        [Fact]
        public async Task HentAlle()
        {
            var port1 = new Port { Id = 1, Name = "Stena1" };
            var port2 = new Port { Id = 2, Name = "ColorLine" };
            var port3 = new Port { Id = 3, Name = "ColorLines" };

            var portListe = new List<Port>();
            portListe.Add(port1);
            portListe.Add(port2);
            portListe.Add(port3);

            mockRep.Setup(k => k.GetAll()).ReturnsAsync(portListe);

            var portController = new PortController(mockRep.Object, mockLog.Object);

            var resultat = await portController.GetAll() as ObjectResult;

            // Assert 
            Assert.Equal<List<Port>>((List<Port>)resultat.Value, portListe);

        }

        [Fact]
        public async Task LagreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Create(portTest)).ReturnsAsync("Port with name: '" + portTest.Name + "' created");

            var portController = new PortController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await portController.Create(portTest) as ObjectResult;

            // Assert 
            Assert.Equal("Port with name: '" + portTest.Name + "' created", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnIkkeOK()
        {
            // Arrange
            var port1 = new Port { Name = "" };
            mockRep.Setup(k => k.Create(port1)).ReturnsAsync("Name is required");

            var portController = new PortController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await portController.Create(port1) as ObjectResult;

            // Assert 
            Assert.Equal("Name is required", resultat.Value);

        }

        [Fact]
        public async Task LagreLoggetInnFeilModel()
        {
            // Arrange
            // Port er indikert feil med tomt Name her.

            var port1 = new Port { Name = "" };

            mockRep.Setup(k => k.Create(port1)).ReturnsAsync("Port with name: '" + port1.Name + "' created");

            var portController = new PortController(mockRep.Object, mockLog.Object);


            portController.ModelState.AddModelError("Name", "Feil i inputvalidering p? server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await portController.Create(port1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering p? server", resultat.Value);
        }

        [Fact]
        public async Task LagreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Create(portTest)).ReturnsAsync("Port with name: '" + portTest.Name + "' created");

            var portController = new PortController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await portController.Create(portTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task SlettLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Port with id: '" + 1 + "' deleted");

            var portController = new PortController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await portController.Delete(1) as ObjectResult;

            // Assert 
            Assert.Equal("Port with id: '" + 1 + "' deleted", resultat.Value);

        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Port with id: '" + 1 + "' deleted");

            var portController = new PortController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await portController.Delete(1) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task HentEnOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(1)).ReturnsAsync(portTest);

            var portController = new PortController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await portController.GetById(1) as ObjectResult;

            // Assert 
            Assert.Equal<Port>(portTest, (Port)resultat.Value);
        }
        
        [Fact]
        public async Task HentEnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(1)).ReturnsAsync(() => null); 

            var portController = new PortController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await portController.GetById(1) as ObjectResult;

            // Assert 
            Assert.Equal(null, resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(1,portTest)).ReturnsAsync("Port with id: '" + 1 + "' updated");

            var portController = new PortController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await portController.Update(1, portTest) as ObjectResult;

            // Assert 
            Assert.Equal("Port with id: '" + 1 + "' updated", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(0, portTest)).ReturnsAsync("Port with id: '" + 0 + "' not found");

            var portController = new PortController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await portController.Update(0, portTest) as ObjectResult;

            // Assert 
            Assert.Equal("Port with id: '" + 0 + "' not found", resultat.Value);

        }

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
            // Arrange
            // Port er indikert feil med tomt Name her.
           
            var port1 = new Port { Name = "" };

            mockRep.Setup(k => k.Update(1,port1)).ReturnsAsync("Port with id: '" + 1 + "' updated");

            var portController = new PortController(mockRep.Object, mockLog.Object);


            portController.ModelState.AddModelError("Name", "Feil i inputvalidering p? server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await portController.Update(1, port1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering p? server", resultat.Value);
        }

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Update(1, portTest)).ReturnsAsync("Port with id: '" + 1 + "' updated");

            var portController = new PortController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            portController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await portController.Update(1, portTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }


    }
}
