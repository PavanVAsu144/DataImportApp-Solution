using DataImportApp.Data;
using DataImportApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace DataImportApp.Tests
{
    public class ProgramTest : IClassFixture<WebApplicationFactory<Program>> // Use WebApplicationFactory to test the full startup
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProgramTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        

     

        

        [Fact]
        public void NLog_Shutdowns_Correctly()
        {
            // Arrange: Get the current logger
            var logger = NLog.LogManager.GetCurrentClassLogger();

            // Act: Log a message and shutdown NLog
            logger.Info("Application is stopping...");
            NLog.LogManager.Shutdown();

            // Assert: No exceptions should be thrown during shutdown
            Assert.True(true); // Ensure that no exceptions were thrown during shutdown
        }
    }
}
