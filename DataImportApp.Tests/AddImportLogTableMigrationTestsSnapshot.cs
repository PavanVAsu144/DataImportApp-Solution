////using System;
//using System.Linq;
//using System.Threading.Tasks;
//using DataImportApp.Data;
//using DataImportApp.Models;
//using FileImportExportAPI.Models;
//using Microsoft.EntityFrameworkCore;
//using Xunit;

//namespace DataImportApp.Tests
//{
//    public class ImportContextTests
//    {
//        private DbContextOptions<ImportContext> CreateNewContextOptions()
//        {
//            // Create a new service provider, and a new context instance using an in-memory database
//            var options = new DbContextOptionsBuilder<ImportContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique db name for the test
//                .Options;

//            return options;
//        }

//        [Fact]
//        public async Task CanAddAndRetrieveImportLog()
//        {
//            // Arrange
//            var options = CreateNewContextOptions();
//            using (var context = new ImportContext(options))
//            {
//                var log = new ImportLog
//                {
//                    FileName = "test.csv",
//                    Message = "Import successful",
//                    Status = "Success",
//                    Timestamp = DateTime.UtcNow,
//                    User = "testuser"
//                };

//                context.ImportLogs.Add(log);
//                await context.SaveChangesAsync();
//            }

//            // Act
//            using (var context = new ImportContext(options))
//            {
//                var log = await context.ImportLogs.FirstOrDefaultAsync();

//                // Assert
//                Assert.NotNull(log);
//                Assert.Equal("test.csv", log.FileName);
//                Assert.Equal("Import successful", log.Message);
//                Assert.Equal("Success", log.Status);
//                Assert.Equal("testuser", log.User);
//            }
//        }

//        [Fact]
//        public async Task CanAddAndRetrieveImportedFile()
//        {
//            // Arrange
//            var options = CreateNewContextOptions();
//            using (var context = new ImportContext(options))
//            {
//                var importedFile = new ImportedFile
//                {
//                    FileName = "data.xlsx",
//                    ImportedOn = DateTime.UtcNow,
//                    UploadedBy = "testuser"
//                };

//                context.ImportedFiles.Add(importedFile);
//                await context.SaveChangesAsync();
//            }

//            // Act
//            using (var context = new ImportContext(options))
//            {
//                var file = await context.ImportedFiles.FirstOrDefaultAsync();

//                // Assert
//                Assert.NotNull(file);
//                Assert.Equal("data.xlsx", file.FileName);
//                Assert.Equal("testuser", file.UploadedBy);
//            }
//        }

//        [Fact]
//        public async Task CanAddAndRetrieveUser()
//        {
//            // Arrange
//            var options = CreateNewContextOptions();
//            using (var context = new ImportContext(options))
//            {
//                var user = new User
//                {
//                    FirstName = "John",
//                    LastName = "Doe",
//                    Email = "john.doe@example.com",
//                    DateOfBirth = new DateTime(1990, 1, 1)
//                };

//                context.Users.Add(user);
//                await context.SaveChangesAsync();
//            }

//            // Act
//            using (var context = new ImportContext(options))
//            {
//                var user = await context.Users.FirstOrDefaultAsync();

//                // Assert
//                Assert.NotNull(user);
//                Assert.Equal("John", user.FirstName);
//                Assert.Equal("Doe", user.LastName);
//                Assert.Equal("john.doe@example.com", user.Email);
//            }
//        }
//    }
//}
