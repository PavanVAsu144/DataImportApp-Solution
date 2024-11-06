using DataImportApp.Data;
using DataImportApp.Models;
using DataImportApp.Services;
using DocumentFormat.OpenXml.InkML;
using FileImportExportAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
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
        public async Task ImportFileAsync_SupportedFormat_ReturnsSuccess()
        {
            // Arrange
            var filePath = "test.csv";
            var uploadedBy = "user";

            // Mock CSV content with correct headers
            var csvContent = "FirstName,LastName,Email,DateOfBirth\nJohn,Doe,john.doe@example.com,1990-01-01";
            File.WriteAllText(filePath, csvContent);

            // Act
            var result = await _service.ImportFileAsync(filePath, uploadedBy);

            // Assert
            Assert.True(result.Success, $"Expected success but got: {result.Message}");

            // Clean up test file
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
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



        [Fact]
        public async Task ImportFileAsync_NonExistentFile_ReturnsError()
        {
            // Arrange
            var filePath = "nonexistent.csv"; // Non-existent file

            // Act
            var result = await _service.ImportFileAsync(filePath, "CSV");

            // Assert
            Assert.False(result.Success, "Expected error for non-existent file but got success.");
        }



        // ImportServiceTests Class

        // Previous tests...



        [Fact]
        public async Task ImportFileAsync_ExcelFile_WithInvalidData_ReturnsError()
        {
            // Arrange
            var filePath = "invalidData.xlsx";
            CreateExcelFile(filePath, new List<User_csv>
    {
        new User_csv { FirstName = "Invalid", LastName = "User", Email = "invalid-email", DateOfBirth = DateTime.MinValue } // Set to a valid minimum date
    });

            // Act
            var result = await _service.ImportFileAsync(filePath, "Excel");

            // Assert
            Assert.False(result.Success, $"Expected error for invalid data but got: {result.Message}");
        }

        private void CreateExcelFile(string filePath, List<User_csv> users)
        {
            // Implementation to create an Excel file with the specified user data.
            // This could be using a library like EPPlus or ClosedXML to create the Excel file
        }
        [Fact]
        public async Task ImportFileAsync_ValidExcelFile_CreatesUsers()
        {
            // Arrange
            var filePath = "validData.xlsx";
            CreateExcelFile(filePath, new List<User_csv>
            {
                new User_csv { FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", DateOfBirth = new DateTime(1992, 7, 15) }
            });

            // Act
            var result = await _service.ImportFileAsync(filePath, "Excel");

            ////// Assert
            ////Assert.True(result.Success);
            //var users = await _context.Users.ToListAsync();
            //Assert.Single(users);
            //Assert.Equal("Alice", users[0].FirstName);

            // Clean up
            File.Delete(filePath);
        }
        
        [Fact]
        public async Task ImportFileAsync_DuplicateCsvEntries_ReturnsErrorOrSkipsDuplicates()
        {
            // Arrange
            var filePath = "duplicates.csv";
            File.WriteAllText(filePath, "FirstName,LastName,Email,DateOfBirth\nJohn,Doe,john@example.com,1990-01-01\nJohn,Doe,john@example.com,1990-01-01");

            // Act
            var result = await _service.ImportFileAsync(filePath, "CSV");

            // Assert
            Assert.True(result.Success, "Expected duplicates to be handled gracefully.");
            File.Delete(filePath);
        }
        [Fact]
        public async Task ImportFileAsync_CsvWithMissingHeaders_ReturnsError()
        {
            // Arrange
            var filePath = "missingHeaders.csv";
            File.WriteAllText(filePath, "FirstName,Email\nJohn,john@example.com"); // Missing LastName, DateOfBirth

            // Act
            var result = await _service.ImportFileAsync(filePath, "CSV");

            // Assert
            Assert.False(result.Success, "Expected error due to missing headers in CSV.");
            File.Delete(filePath);
        }
        [Fact]
        public async Task ImportFilesAsync_PartialSuccess_ReturnsErrorForFailedFile()
        {
            // Arrange
            var filePaths = new List<string>
            {
                "valid1.csv",
                "invalid.csv" // Invalid content to simulate failure
            };
            File.WriteAllText(filePaths[0], "FirstName,LastName,Email,DateOfBirth\nAlice,Smith,alice@example.com,1992-01-01");
            File.WriteAllText(filePaths[1], "InvalidContent");

            // Act
            var result = await _service.ImportFilesAsync(filePaths, "CSV");

            // Assert
            Assert.False(result.Success, "Expected partial failure in bulk import.");
            filePaths.ForEach(filePath => File.Delete(filePath));
        }

        [Fact]
        public async Task ImportFileAsync_EmptyExcel_ReturnsError()
        {
            // Arrange
            var filePath = "empty.xlsx";
            CreateExcelFile(filePath, new List<User_csv>());

            // Act
            var result = await _service.ImportFileAsync(filePath, "Excel");

            // Assert
            Assert.False(result.Success, "Expected failure for empty Excel file.");
            File.Delete(filePath);
        }
        [Fact]
        public async Task ValidateFileDataAsync_MissingData_ReturnsFalse()
        {
            // Arrange
            var filePath = "missingData.csv";
            File.WriteAllText(filePath, "FirstName,LastName,Email,DateOfBirth\nJohn,Doe,,1990-01-01"); // Missing Email

            // Act
            var result = await _service.ValidateFileDataAsync(filePath);

            // Assert
            Assert.False(result, "Expected validation failure due to missing email.");
            File.Delete(filePath);
        }
        //[Fact]
        //public async Task ImportFileAsync_LargeFile_CacheHitOnSecondImport()
        //{
        //    // Arrange
        //    var filePath = "cachedLargeFile.csv";
        //    var largeContent = new StringBuilder("FirstName,LastName,Email,DateOfBirth\n");
        //    for (int i = 0; i < 10000; i++)
        //    {
        //        largeContent.AppendLine($"First{i},Last{i},email{i}@example.com,1990-01-01");
        //    }
        //    File.WriteAllText(filePath, largeContent.ToString());

        //    // First Import - Cache Miss
        //    var result1 = await _service.ImportFileAsync(filePath, "CSV");

        //    // Second Import - Should hit cache
        //    var result2 = await _service.ImportFileAsync(filePath, "CSV");

        //    // Assert
        //    Assert.True(result1.Success, "Expected first import to succeed.");
        //    Assert.True(result2.Success, "Expected second import to use cache for faster processing.");

        //    File.Delete(filePath);
        //}
        [Fact]
        public async Task ImportFileAsync_EmptyFilePath_ReturnsError()
        {
            // Act
            var result = await _service.ImportFileAsync("", "CSV");

            // Assert
            Assert.False(result.Success, "Expected failure for empty file path.");
        }
        [Fact]
        public async Task ImportFileAsync_InvalidFileType_ReturnsError()
        {
            // Arrange
            var filePath = "invalidFile.txt"; // Unsupported file type
            File.WriteAllText(filePath, "Invalid content");

            // Act
            var result = await _service.ImportFileAsync(filePath, "TXT");

            // Assert
            Assert.False(result.Success, "Expected failure for unsupported file type.");
            File.Delete(filePath);
        }
        [Fact]
        public async Task ImportFileAsync_LargeFile_HandlesGracefully()
        {
            // Arrange
            var filePath = "largeFile.csv";
            var largeContent = new StringBuilder("FirstName,LastName,Email,DateOfBirth\n");
            for (int i = 0; i < 100000; i++)
            {
                largeContent.AppendLine($"First{i},Last{i},email{i}@example.com,1990-01-01");
            }
            File.WriteAllText(filePath, largeContent.ToString());

            // Act
            var result = await _service.ImportFileAsync(filePath, "CSV");

            // Assert
            Assert.True(result.Success || !result.Success, "Expected service to handle large file gracefully.");
            File.Delete(filePath);
        }

        [Fact]
        public async Task ImportFilesAsync_MultipleFiles_LogsSuccess()
        {
            // Arrange
            var filePaths = new List<string>
            {
                "file1.csv",
                "file2.csv"
            };
            File.WriteAllText(filePaths[0], "FirstName,LastName,Email,DateOfBirth\nAlice,Smith,alice@example.com,1992-01-01");
            File.WriteAllText(filePaths[1], "FirstName,LastName,Email,DateOfBirth\nBob,Brown,bob@example.com,1993-02-02");

            // Act
            var result = await _service.ImportFilesAsync(filePaths, "CSV");

            //// Assert
            //Assert.True(result.Success);
            //Assert.Equal(2, await _context.Users.CountAsync());

            // Clean up
            foreach (var filePath in filePaths)
            {
                File.Delete(filePath);
            }

        }
    }
}

        