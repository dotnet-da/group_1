using backend.Controllers;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Logging.Abstractions;

namespace backend.tests
{
    [TestClass]
    public class WeatherForecastControllerTests
    {
        [TestMethod]
        public void CheckForData()
        {
            // Arrange
            var controller = new WeatherForecastController(new NullLogger<WeatherForecastController>());

            // Act
            var response = controller.GetWeatherForecast();

            // Assert
            Assert.IsTrue(response.Any());
        }
    }
}