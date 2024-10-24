using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using DataImportApp; // Ensure this namespace is included
using DataImportApp;
using DataImportApp.Controllers;

namespace DataImportApp.Tests.Controllers
{
    public class WeatherForecastControllerTests
    {
        private readonly Mock<ILogger<WeatherForecastController>> _mockLogger;
        private readonly WeatherForecastController _controller;

        public WeatherForecastControllerTests()
        {
            _mockLogger = new Mock<ILogger<WeatherForecastController>>();
            _controller = new WeatherForecastController(_mockLogger.Object);
        }

        [Fact]
        public void Get_ReturnsWeatherForecasts()
        {
            // Act
            var result = _controller.Get();

            // Assert
            var okResult = Assert.IsType<WeatherForecast[]>(result); // Ensure we assert the correct type
            Assert.Equal(5, okResult.Length);

            foreach (var forecast in okResult)
            {
                Assert.IsType<DateOnly>(forecast.Date);
                Assert.InRange(forecast.TemperatureC, -20, 55); // Ensure temperature is within range
                Assert.Contains(forecast.Summary, new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" });
            }
        }
    }
}
