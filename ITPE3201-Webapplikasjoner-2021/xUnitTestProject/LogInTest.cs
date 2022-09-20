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
using Microsoft.Extensions.DependencyInjection;

namespace GruppeoppgaveTest
{

    public class LogIn
    {

        private readonly Mock<IUserRepository> mockRep = new Mock<IUserRepository>();
        private readonly Mock<ILogger<UserController>> mockLog = new Mock<ILogger<UserController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();



        //[Fact]
        //public async Task LoggInnOK()
        //{
        //    InputUser user1 = new InputUser
        //    {
        //        Username = "admin",
        //        Password = "admin123"
        //    };

            mockRep.Setup(k => k.Login(user1)).ReturnsAsync(true);

        //    var userController = new UserController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            userController.ControllerContext.HttpContext = mockHttpContext.Object;
            // Act
            var resultat = await userController.Login(user1) as ObjectResult;

            // Assert 
            Assert.Equal("Valid user credentials, logged in", resultat.Value);
        }

        [Fact]
        public async Task LoggInnFeilPassordEllerBruker()
        {
            InputUser user1 = new InputUser
            {
                Username = "admins", //Feil Username
                Password = "admin123",
            };
            mockRep.Setup(k => k.Login(user1));

            var userController = new UserController(mockRep.Object, mockLog.Object);

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            userController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await userController.Login(user1) as ObjectResult;

            // Assert 
            Assert.Equal("Invalid user credentials", resultat.Value);
        }

        [Fact]
        public async Task LoggInnInputFeil()
        {
            InputUser user1 = new InputUser
            {
                Username = "admin",
                Password = "admin123",
            };
            mockRep.Setup(k => k.Login(user1)).ReturnsAsync(true);

            var userController = new UserController(mockRep.Object, mockLog.Object);

            userController.ModelState.AddModelError("Username", "Invalid user credentials");

            mockSession.SetString("_loggedIn", "true");
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            userController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await userController.Login(user1) as ObjectResult;

            // Assert 

            Assert.Equal("Invalid user credentials", resultat.Value);
        }

        [Fact]
        public void LoggUt()
        {
            var userController = new UserController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession.SetString("_loggedIn", "true");
            userController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            userController.Logout();

            // Assert
            Assert.Equal("", mockSession["_loggedIn"]);

        }
    }
}