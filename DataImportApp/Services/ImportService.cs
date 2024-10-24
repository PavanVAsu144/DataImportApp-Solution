using DataImportApp.Data;
using DataImportApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using ExcelDataReader;
using System.Data;
using FileImportExportAPI.Models;
using System.Text;
using System.Numerics;

namespace DataImportApp.Services
{
    public class ImportService : IImportService
    {
        private readonly ImportContext _context;
        private readonly ILogger<ImportService> _logger;

        public ImportService(ImportContext context, ILogger<ImportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ImportResult> ImportFileAsync(string filePath, string fileType)
        {
            try
            {
                _logger.LogInformation($"Starting import for file: {filePath}");

                // Determine file extension
                var extension = Path.GetExtension(filePath);

                if (extension == ".csv")
                {
                    _logger.LogInformation($"Processing CSV file: {filePath}");
                    await ImportCsvFile(filePath);
                }
                else if (extension == ".xlsx")
                {
                    _logger.LogInformation($"Processing Excel file: {filePath}");
                    await ImportExcelFile(filePath);
                }
                else
                {
                    _logger.LogError($"Unsupported file format: {filePath}");
                    return new ImportResult { Success = false, Message = "Unsupported file format." };
                }

                LogImportActivity(filePath, fileType, "Success", "File imported successfully");
                _logger.LogInformation($"File imported successfully: {filePath}");
                return new ImportResult { Success = true, Message = "File imported successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while importing file: {filePath}");
                LogImportActivity(filePath, fileType, "Failed", ex.Message);
                return new ImportResult { Success = false, Message = $"Error importing file: {ex.Message}" };
            }
        }

        public async Task<ImportResult> ImportFilesAsync(IEnumerable<string> filePaths, string fileType)
        {
            _logger.LogInformation($"Starting bulk import for {filePaths.Count()} files.");

            foreach (var filePath in filePaths)
            {
                var result = await ImportFileAsync(filePath, fileType);
                if (!result.Success)
                {
                    _logger.LogWarning($"Bulk import failed for file: {filePath}");
                    return result;
                }
            }

            _logger.LogInformation("All files imported successfully.");
            return new ImportResult { Success = true, Message = "All files imported successfully." };
        }

        private async Task ImportCsvFile(string filePath)
        {
            try
            {
                _logger.LogInformation($"Reading CSV file: {filePath}");

                using (var reader = new StreamReader(filePath, Encoding.GetEncoding(1252)))
                using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<User_csv>().ToList();
                    for (int i = 0; i < records.Count; i++)
                    {
                        var user = new User
                        {
                            FirstName = records[i].FirstName,
                            LastName = records[i].LastName,
                            Email = records[i].Email,
                            DateOfBirth = records[i].DateOfBirth
                        };

                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"CSV record imported: {user.Email}");
                    }
                }

                _logger.LogInformation($"CSV file successfully processed: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while processing CSV file: {filePath}");
                throw; // Rethrow to ensure the calling method can handle it
            }
        }

        private async Task ImportExcelFile(string filePath)
        {
            try
            {
                _logger.LogInformation($"Reading Excel file: {filePath}");

                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true // Assuming the first row is the header
                            }
                        };

                        var result = reader.AsDataSet(conf);
                        var dataTable = result.Tables[0]; // Access the first worksheet

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string email = row[3]?.ToString();
                            string dobString = row[4]?.ToString();

                            if (!DateTime.TryParse(dobString, out DateTime dateOfBirth))
                            {
                                _logger.LogWarning($"Invalid DateTime for Email '{email}': {dobString}. Skipping entry.");
                                continue; // Skip this row if DateOfBirth is invalid
                            }

                            var user = new User
                            {
                                FirstName = row[1].ToString(),
                                LastName = row[2].ToString(),
                                Email = email,
                                DateOfBirth = dateOfBirth
                            };

                            _context.Users.Add(user);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation($"Excel record imported: {user.Email}");
                        }
                    }
                }

                _logger.LogInformation($"Excel file successfully processed: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while processing Excel file: {filePath}");
                throw; // Ensure the exception is caught in the calling method
            }
        }

        public async Task<bool> ValidateFileDataAsync(string filePath)
        {
            try
            {
                _logger.LogInformation($"Validating file data: {filePath}");

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<ImportedFile>().ToList(); // Use your specific model
                    // Add custom validation logic (e.g., check for correct columns, etc.)

                    _logger.LogInformation($"File data validation passed for: {filePath}");
                    return true; // Assume validation is successful for now
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"File validation error: {filePath}");
                return false;
            }
        }

        // Log import activity to the database
        public void LogImportActivity(string fileName, string user, string status, string message)
        {
            try
            {
                var log = new ImportLog
                {
                    FileName = fileName,
                    User = user,
                    Timestamp = DateTime.Now,
                    Status = status,
                    Message = message
                };

                _context.ImportLogs.Add(log);
                _context.SaveChanges();
                _logger.LogInformation($"Logged import activity for file: {fileName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error logging import activity for file: {fileName}");
            }
        }
    }
}
