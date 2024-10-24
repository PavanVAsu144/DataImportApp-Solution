using System;
using DataImportApp.Models;
using Xunit;

namespace DataImportApp.Tests.Models
{
    public class ImportLogTests
    {
        [Fact]
        public void ImportLog_Creation_SetsPropertiesCorrectly()
        {
            // Arrange
            var expectedId = 1;
            var expectedFileName = "test_file.csv";
            var expectedUser = "user@example.com";
            var expectedTimestamp = DateTime.Now;
            var expectedStatus = "Success";
            var expectedMessage = "File imported successfully.";

            // Act
            var importLog = new ImportLog
            {
                Id = expectedId,
                FileName = expectedFileName,
                User = expectedUser,
                Timestamp = expectedTimestamp,
                Status = expectedStatus,
                Message = expectedMessage
            };

            // Assert
            Assert.Equal(expectedId, importLog.Id);
            Assert.Equal(expectedFileName, importLog.FileName);
            Assert.Equal(expectedUser, importLog.User);
            Assert.Equal(expectedTimestamp.Date, importLog.Timestamp.Date); // Compare only date part
            Assert.Equal(expectedStatus, importLog.Status);
            Assert.Equal(expectedMessage, importLog.Message);
        }
    }
}
