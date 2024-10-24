using DataImportApp.Data;
using DataImportApp.Models;
using DataImportApp.Services;
using FileImportExportAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataImportApp.Services
{
    public class ImportServiceTests
    {
        private readonly ImportContext _context;
        private readonly Mock<ILogger<ImportService>> _mockLogger;
        private readonly ImportService _service;

        public ImportServiceTests()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var options = new DbContextOptionsBuilder<ImportContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ImportContext(options);
            _mockLogger = new Mock<ILogger<ImportService>>();
            _service = new ImportService(_context, _mockLogger.Object);
        }
      

        [Fact]
        public async Task ImportFilesAsyncValidFilesReturnsSuccess()
        {
            // Arrange
            var filePaths = "test.csv";// Ensure these files exist or mock their content
            var uploadedBy = "user";

            // Mock successful import for multiple files
            foreach (var path in filePaths)
            {
                // You might want to set up any expected behavior here for each file
            }

            // Act
            var result = await _service.ImportFileAsync(filePaths, uploadedBy);


            // Assert
            Assert.True(result.Success, $"Expected success but got: {result.Message}");
        }

        [Fact]
        public async Task ImportFileAsync_SupportedFormat_ReturnsSuccess()
        {
            // Arrange
            var filePath = "test.csv"; // Ensure this file exists or mock its content
            var uploadedBy = "user";

            // Mock any necessary setup in ImportContext or ImportService methods
            // Ensure the file content is valid for testing.

            // Act
            var result = await _service.ImportFileAsync(filePath, uploadedBy);

            // Assert
            Assert.True(result.Success, $"Expected success but got: {result.Message}");
        }




        [Fact]
        public async Task ImportFileAsync_UnsupportedFormat_ReturnsError()
        {
            // Arrange
            var filePath = "test.txt"; // Unsupported file format
            var fileType = "TXT";

            // Act
            var result = await _service.ImportFileAsync(filePath, fileType);

            // Assert
            Assert.False(result.Success, result.Message);
        }

        [Fact]
        public async Task ImportFileAsync_InvalidCsvFile_ReturnsError()
        {
            // Arrange
            var filePath = "invalid.csv"; // Create a CSV file with invalid content
            File.WriteAllText(filePath, "FirstName,LastName,Email\nJohn,Doe,john.doe@example.com"); // Missing DateOfBirth

            // Act
            var result = await _service.ImportFileAsync(filePath, "CSV");

            // Assert
            Assert.False(result.Success, result.Message);
        }

        
        [Fact]
        public async Task ValidateFileDataAsync_InvalidFile_ReturnsFalse()
        {
            // Arrange
            var filePath = "invalid.csv"; // Create a CSV file with invalid content
            File.WriteAllText(filePath, "FirstName,LastName,Email\nInvalid,User,invalid@example.com"); // Missing DateOfBirth

            // Act
            var result = await _service.ValidateFileDataAsync(filePath);

            // Assert
            Assert.False(result);
        }

        

        // New Test Cases


        [Fact]
        public async Task ImportFileAsync_MissingColumnsInCsv_ReturnsError()
        {
            // Arrange
            var filePath = "missingColumns.csv";
            File.WriteAllText(filePath, "FirstName,Email\nJohn,john@example.com"); // Missing LastName and DateOfBirth

            // Act
            var result = await _service.ImportFileAsync(filePath, "CSV");

            // Assert
            Assert.False(result.Success, result.Message);
        }
      
        private void CreateExcelFile(string filePath, List<User_csv> users)
        {
            // Implementation to create an Excel file with the specified user data.
        }
    }
}
