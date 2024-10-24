using DataImportApp;
using DataImportApp.Data;
using DataImportApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;

namespace DataImportApp.Tests
{
    public class StartupTests
    {
        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddScoped<IImportService, ImportService>();
                    services.AddDbContext<ImportContext>(options =>
                        options.UseInMemoryDatabase("TestDatabase"));
                });
        }

        [Fact]
        public void ConfigureServices_RegistersServicesCorrectly()
        {
            // Arrange
            var host = CreateHostBuilder().Start();

            // Act
            var serviceProvider = host.Services;

            // Assert
            var importService = serviceProvider.GetService<IImportService>();
            Assert.NotNull(importService); // Ensure that the service is registered

            var importContext = serviceProvider.GetService<ImportContext>();
            Assert.NotNull(importContext); // Ensure that the context is registered
        }

        [Fact]
        public void ConfigureServices_ConfiguresDatabaseContextCorrectly()
        {
            // Arrange
            var host = CreateHostBuilder().Start();

            // Act
            var serviceProvider = host.Services;
            var dbContext = serviceProvider.GetService<ImportContext>();

            // Assert
            Assert.NotNull(dbContext);
            Assert.IsType<ImportContext>(dbContext); // Ensure that the context is of the correct type
        }
    }
}
