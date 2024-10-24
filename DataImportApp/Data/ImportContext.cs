using Microsoft.EntityFrameworkCore;
using DataImportApp.Models;
using FileImportExportAPI.Models;
using DataImportApp.Services;

namespace DataImportApp.Data
{
    public class ImportContext : DbContext
    {
        public ImportContext(DbContextOptions<ImportContext> options) : base(options) { }

        public DbSet<ImportedFile> ImportedFiles { get; set; }
        public DbSet<ImportLog> ImportLogs { get; set; }
        public DbSet<User> Users { get; set; }
       
    }
}
