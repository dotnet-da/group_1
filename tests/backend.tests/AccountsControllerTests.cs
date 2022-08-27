using backend.Controllers;
using backend.Helpers;
using backend.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace backend.tests
{
    [TestClass]
    public class AccountsControllerTests
    {
        public static AppSettings _appSettings { get; private set; }
        public static AccountsManagementService _accountsManagementService { get; private set; }
        public static AccountsController _accountsController { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            _appSettings = new AppSettings { Secret="Test secret-----"};
            IOptions<AppSettings> appSettings = Options.Create(_appSettings);
            _accountsManagementService = new AccountsManagementService(appSettings);
            _accountsController = new AccountsController(_accountsManagementService);
        }

        [TestMethod]
        public void AdminAuthentication()
        {
            var request = new AccountsDLL.Models.AuthenticateRequest
            {
                Username = "admin",
                Password = "admin"
            };

            // Act
            var response = _accountsController.Authenticate(request);

            // Assert
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
        }

        [TestMethod]
        public void WrongPassword()
        {
            var request = new AccountsDLL.Models.AuthenticateRequest
            {
                Username = "admin",
                Password = "admin2"
            };

            // Act
            var response = _accountsController.Authenticate(request);

            // Assert
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult));
        }

    }
}