using System;
using DataImportApp.Models;
using Xunit;

namespace DataImportApp.Tests.Models
{
    public class ImportedFileTests
    {
        [Fact]
        public void ImportedFile_Creation_SetsPropertiesCorrectly()
        {
            // Arrange
            var expectedId = 1;
            var expectedFileName = "test_file.csv";
            var expectedImportedOn = DateTime.Now;
            var expectedUploadedBy = "user@example.com";

            // Act
            var importedFile = new ImportedFile
            {
                Id = expectedId,
                FileName = expectedFileName,
                ImportedOn = expectedImportedOn,
                UploadedBy = expectedUploadedBy
            };

            // Assert
            Assert.Equal(expectedId, importedFile.Id);
            Assert.Equal(expectedFileName, importedFile.FileName);
            Assert.Equal(expectedImportedOn.Date, importedFile.ImportedOn.Date); // Compare only date part
            Assert.Equal(expectedUploadedBy, importedFile.UploadedBy);
        }
    }
}
