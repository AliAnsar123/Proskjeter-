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

    public class CompanyTest
    {

        private readonly Mock<ICompanyRepository> mockRep = new Mock<ICompanyRepository>();
        private readonly Mock<ILogger<CompanyController>> mockLog = new Mock<ILogger<CompanyController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();
        private readonly Company companyTest = new Company { Name = "Stenaa" };



        [Fact]
        public async Task HentAlle()
        {
            var company1 = new Company { Id = 1, Name = "Stena1" };
            var company2 = new Company { Id = 2, Name = "ColorLine" };
            var company3 = new Company { Id = 3, Name = "ColorLines" };

            var companyListe = new List<Company>();
            companyListe.Add(company1);
            companyListe.Add(company2);
            companyListe.Add(company3);

            mockRep.Setup(k => k.GetAll()).ReturnsAsync(companyListe);

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            var resultat = await companyController.GetAll() as ObjectResult;

            // Assert 
            Assert.Equal<List<Company>>((List<Company>)resultat.Value, companyListe);

        }

        [Fact]
        public async Task LagreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Create(companyTest)).ReturnsAsync("Company with name: '" + companyTest.Name + "' created");

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            companyController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await companyController.Create(companyTest) as ObjectResult;

            // Assert 
            Assert.Equal("Company with name: '" + companyTest.Name + "' created", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnIkkeOK()
        {
            // Arrange
            var company1 = new Company { Name = "" };
            mockRep.Setup(k => k.Create(company1)).ReturnsAsync("Name is required");

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            companyController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await companyController.Create(company1) as ObjectResult;

            // Assert 
            Assert.Equal("Name is required", resultat.Value);

        }

        [Fact]
        public async Task LagreLoggetInnFeilModel()
        {
            // Arrange
            // Company er indikert feil med tomt Name her.

            var company1 = new Company { Name = "" };

            mockRep.Setup(k => k.Create(company1)).ReturnsAsync("Company with name: '" + company1.Name + "' created");

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);


            companyController.ModelState.AddModelError("Name", "Feil i inputvalidering p? server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            companyController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await companyController.Create(company1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering p? server", resultat.Value);
        }

        [Fact]
        public async Task LagreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Create(companyTest)).ReturnsAsync("Company with name: '" + companyTest.Name + "' created");

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            companyController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await companyController.Create(companyTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task SlettLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Company with id: '" + 1 + "' deleted");

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            companyController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await companyController.Delete(1) as ObjectResult;

            // Assert 
            Assert.Equal("Company with id: '" + 1 + "' deleted", resultat.Value);

        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {

            mockRep.Setup(k => k.Delete(1)).ReturnsAsync("Company with id: '" + 1 + "' deleted");

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            companyController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await companyController.Delete(1) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }

        [Fact]
        public async Task HentEnOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(1)).ReturnsAsync(companyTest);

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await companyController.GetById(1) as ObjectResult;

            // Assert 
            Assert.Equal<Company>(companyTest, (Company)resultat.Value);
        }
        
        [Fact]
        public async Task HentEnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.GetById(1)).ReturnsAsync(() => null); 

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await companyController.GetById(1) as ObjectResult;

            // Assert 
            Assert.Null(resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(1,companyTest)).ReturnsAsync("Company with id: '" + 1 + "' updated");

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            companyController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await companyController.Update(1, companyTest) as ObjectResult;

            // Assert 
            Assert.Equal("Company with id: '" + 1 + "' updated", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.Update(0, companyTest)).ReturnsAsync("Company with id: '" + 0 + "' not found");

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            companyController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await companyController.Update(0, companyTest) as ObjectResult;

            // Assert 
            Assert.Equal("Company with id: '" + 0 + "' not found", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
            // Arrange
            // Company er indikert feil med tomt Name her.
           
            var company1 = new Company { Name = "" };

            mockRep.Setup(k => k.Update(1,company1)).ReturnsAsync("Company with id: '" + 1 + "' updated");

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);


            companyController.ModelState.AddModelError("Name", "Feil i inputvalidering på server");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            companyController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await companyController.Update(1, company1) as ObjectResult;

            // Assert 
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Update(1, companyTest)).ReturnsAsync("Company with id: '" + 1 + "' updated");

            var companyController = new CompanyController(mockRep.Object, mockLog.Object);

            mockSession["_loggedIn"] = "";
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            companyController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await companyController.Update(1, companyTest) as ObjectResult;

            // Assert 
            Assert.Equal("Unauthorized", resultat.Value);
        }


    }
}
