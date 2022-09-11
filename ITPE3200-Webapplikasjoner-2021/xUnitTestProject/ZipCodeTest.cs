
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

    public class ZipCodeTest
    {

        private readonly Mock<IZipCodeRepository> mockRep = new Mock<IZipCodeRepository>();
        private readonly Mock<ILogger<ZipCodeController>> mockLog = new Mock<ILogger<ZipCodeController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();
        private readonly ZipCode zipCodeTest = new ZipCode { City = "Bloksberg" };
        private readonly string id = "3012";



        [Fact]
        public async Task HentAlle()
        {
            var zipCode1 = new ZipCode { Id = "4790", City = "Lillesand" };
            var zipCode2 = new ZipCode { Id = "3928", City = "Porsgrunn" };
            var zipCode3 = new ZipCode { Id = "9482", City = "Harstad" };

            var zipCodeListe = new List<ZipCode>();
            zipCodeListe.Add(zipCode1);
            zipCodeListe.Add(zipCode2);
            zipCodeListe.Add(zipCode3);

            mockRep.Setup(k => k.GetAll()).ReturnsAsync(zipCodeListe);

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            var resultat = await zipCodeController.GetAll() as ObjectResult;

            // Assert 
            Assert.Equal<List<ZipCode>>((List<ZipCode>)resultat.Value, zipCodeListe);

        }

        [Fact]
        public async Task LagreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Create(zipCodeTest)).ReturnsAsync("ZipCode with name: '" + zipCodeTest.City + "' created");

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            zipCodeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await zipCodeController.Create(zipCodeTest) as ObjectResult;

            // Assert 
            Assert.Equal("ZipCode with name: '" + zipCodeTest.City + "' created", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnIkkeOK()
        {
            // Arrange
            var zipCode1 = new ZipCode { City = "" };
            mockRep.Setup(k => k.Create(zipCode1)).ReturnsAsync("City is required");

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            zipCodeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await zipCodeController.Create(zipCode1) as ObjectResult;

            // Assert 
            Assert.Equal("City is required", resultat.Value);

        }

        [Fact]
        public async Task LagreLoggetInnFeilModel()
        {
            // Arrange
            // ZipCode er indikert feil med tomt City her.

            var zipCode1 = new ZipCode { City = "" };

            mockRep.Setup(k => k.Create(zipCode1)).ReturnsAsync("ZipCode with name: '" + zipCode1.City + "' created");

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);


            zipCodeController.ModelState.AddModelError("City", "Feil i inputvalidering på server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            zipCodeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await zipCodeController.Create(zipCode1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task LagreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Create(zipCodeTest)).ReturnsAsync("ZipCode with name: '" + zipCodeTest.City + "' created");

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            zipCodeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await zipCodeController.Create(zipCodeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task SlettLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Delete(id)).ReturnsAsync("ZipCode with id: '" + id + "' deleted");

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            zipCodeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await zipCodeController.Delete(id) as ObjectResult;

            // Assert 
            Assert.Equal("ZipCode with id: '" + id + "' deleted", resultat.Value);

        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {

            mockRep.Setup(k => k.Delete(id)).ReturnsAsync("ZipCode with id: '" + id + "' deleted");

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            zipCodeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await zipCodeController.Delete(id) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task HentEnOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(id)).ReturnsAsync(zipCodeTest);

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await zipCodeController.GetById(id) as ObjectResult;

            // Assert 
            Assert.Equal<ZipCode>(zipCodeTest, (ZipCode)resultat.Value);
        }
        
        [Fact]
        public async Task HentEnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(id)).ReturnsAsync(() => null); 

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await zipCodeController.GetById(id) as ObjectResult;

            // Assert 
            Assert.Equal(null, resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(id,zipCodeTest)).ReturnsAsync("ZipCode with id: '" + id + "' updated");

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            zipCodeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await zipCodeController.Update(id, zipCodeTest) as ObjectResult;

            // Assert 
            Assert.Equal("ZipCode with id: '" + id + "' updated", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(id, zipCodeTest)).ReturnsAsync("ZipCode with id: '" + id + "' not found");

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            zipCodeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await zipCodeController.Update(id, zipCodeTest) as ObjectResult;

            // Assert 
            Assert.Equal("ZipCode with id: '" + id + "' not found", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
            // Arrange
            // ZipCode er indikert feil med tomt City her.
           
            var zipCode1 = new ZipCode { City = "" };

            mockRep.Setup(k => k.Update(id,zipCode1)).ReturnsAsync("ZipCode with id: '" + id + "' updated");

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);


            zipCodeController.ModelState.AddModelError("City", "Feil i inputvalidering på server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            zipCodeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await zipCodeController.Update(id, zipCode1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Update(id, zipCodeTest)).ReturnsAsync("ZipCode with id: '" + id + "' updated");

            var zipCodeController = new ZipCodeController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            zipCodeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await zipCodeController.Update(id, zipCodeTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }


    }
}
