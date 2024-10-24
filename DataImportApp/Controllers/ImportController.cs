using DataImportApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataImportApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;
        private readonly string _uploadFolder;

        public ImportController(IImportService importService)
        {
            _importService = importService;
            // Define the folder where files will be saved (inside the application root directory)
            _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportFiles([FromForm] IFormFile[] files)
        {
            // Scenario 1: No files uploaded
            if (files == null || files.Length == 0)
            {
                return BadRequest("No files uploaded.");
            }

            var filePaths = files.Select(file =>
            {
                // Save each file in the "Uploads" folder
                var filePath = Path.Combine(_uploadFolder, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return filePath;
            }).ToList();

            // Scenario 2: Import service returns a failure
            var result = await _importService.ImportFilesAsync(filePaths, "user");
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }

            // Scenario 3: Successful import
            return Ok(result.Message);
        }
    }
}
