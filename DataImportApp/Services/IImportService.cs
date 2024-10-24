using System.Collections.Generic;
using System.Threading.Tasks;
using DataImportApp.Models;

namespace DataImportApp.Services
{
    public interface IImportService
    {
        Task<ImportResult> ImportFileAsync(string filePath, string uploadedBy);
        Task<ImportResult> ImportFilesAsync(IEnumerable<string> filePaths, string uploadedBy);
        Task<bool> ValidateFileDataAsync(string filePath);
    }
}
