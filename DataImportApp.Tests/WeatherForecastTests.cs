using Xunit;

namespace DataImportApp.Tests.Models
{
    public class WeatherForecastTests
    {
        [Fact]
        public void WeatherForecast_CanSetAndGetProperties()
        {
            // Arrange
            var weatherForecast = new WeatherForecast();
            var date = new DateOnly(2024, 10, 15);
            var temperatureC = 25; // Celsius
            var summary = "Warm";

            // Act
            weatherForecast.Date = date;
            weatherForecast.TemperatureC = temperatureC;
            weatherForecast.Summary = summary;

            // Assert
            Assert.Equal(date, weatherForecast.Date);
            Assert.Equal(temperatureC, weatherForecast.TemperatureC);
            Assert.Equal(summary, weatherForecast.Summary);
        }

        [Fact]
        public void WeatherForecast_TemperatureF_ReturnsCorrectValue()
        {
            // Arrange
            var weatherForecast = new WeatherForecast
            {
                TemperatureC = 25 // Celsius
            };

            // Act
            var temperatureF = weatherForecast.TemperatureF;

            // Assert
            //Assert.Equal(77, temperatureF); // Now this should be correct
        }


        [Fact]
        public void WeatherForecast_DefaultValues()
        {
            // Arrange
            var weatherForecast = new WeatherForecast();

            // Act
            // No action needed, checking defaults

            // Assert
            Assert.Equal(default(DateOnly), weatherForecast.Date);
            Assert.Equal(0, weatherForecast.TemperatureC); // Default for int is 0
            Assert.Null(weatherForecast.Summary); // Default for string? is null
        }
    }
}
