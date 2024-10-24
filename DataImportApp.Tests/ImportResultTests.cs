using DataImportApp.Models;
using Xunit;

namespace DataImportApp.Tests.Models
{
    public class ImportResultTests
    {
        [Fact]
        public void ImportResult_ShouldSetAndGetSuccessProperty()
        {
            // Arrange
            var importResult = new ImportResult();

            // Act
            importResult.Success = true;

            // Assert
            Assert.True(importResult.Success);
        }

        [Fact]
        public void ImportResult_ShouldSetAndGetMessageProperty()
        {
            // Arrange
            var importResult = new ImportResult();
            var expectedMessage = "Import completed successfully.";

            // Act
            importResult.Message = expectedMessage;

            // Assert
            Assert.Equal(expectedMessage, importResult.Message);
        }

        [Fact]
        public void ImportResult_ShouldDefaultProperties()
        {
            // Arrange
            var importResult = new ImportResult();

            // Act
            // No action needed as we are checking defaults

            // Assert
            Assert.False(importResult.Success); // Default value for bool is false
            Assert.Null(importResult.Message);   // Default value for string is null
        }
    }
}
