using DataImportApp.Controllers;
using DataImportApp.Models;
using DataImportApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

public class ImportControllerTests
{
    private readonly Mock<IImportService> _mockImportService;
    private readonly ImportController _controller;

    public ImportControllerTests()
    {
        _mockImportService = new Mock<IImportService>();
        _controller = new ImportController(_mockImportService.Object);
    }

    [Fact]
    public async Task ImportFiles_NoFilesUploaded_ReturnsBadRequest()
    {
        // Arrange
        IFormFile[] files = null;

        // Act
        var result = await _controller.ImportFiles(files);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("No files uploaded.", badRequestResult.Value);
    }

    [Fact]
    public async Task ImportFiles_SuccessfulImport_ReturnsOk()
    {
        // Arrange
        var files = new IFormFile[]
        {
            new FormFile(new MemoryStream(), 0, 10, "file", "test.csv")
        };
        _mockImportService.Setup(x => x.ImportFilesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .ReturnsAsync(new ImportResult { Success = true, Message = "Files Imported Successfully" });

        // Act
        var result = await _controller.ImportFiles(files);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Files Imported Successfully", okResult.Value);
    }

    [Fact]
    public async Task ImportFiles_ImportServiceFailure_ReturnsInternalServerError()
    {
        // Arrange
        var files = new IFormFile[]
        {
            new FormFile(new MemoryStream(), 0, 10, "file", "test.csv")
        };
        _mockImportService.Setup(x => x.ImportFilesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
            .ReturnsAsync(new ImportResult { Success = false, Message = "Import failed." });

        // Act
        var result = await _controller.ImportFiles(files);

        // Assert
        var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
        Assert.Equal("Import failed.", internalServerErrorResult.Value);
    }
}
