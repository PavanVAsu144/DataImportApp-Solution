using DataImportApp.Data;
using DataImportApp.Services;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using System.Text;

// Register the CodePagesEncodingProvider for handling different encodings
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Setup NLog for dependency injection
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
logger.Info("Application is starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container
    builder.Services.AddScoped<IImportService, ImportService>(); // Register ImportService
    builder.Services.AddControllers();                           // Add controller services
    builder.Services.AddEndpointsApiExplorer();                  // Enable endpoint exploration
    builder.Services.AddSwaggerGen();                            // Enable Swagger

    // Configure Entity Framework with SQL Server
    builder.Services.AddDbContext<ImportContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Clear default logging providers and add NLog
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); // Set minimum log level
    builder.Host.UseNLog();  // Use NLog as the logging provider

    var app = builder.Build();

    // Configure the HTTP request pipeline for development
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();    // Enable Swagger in development mode
        app.UseSwaggerUI();  // Swagger UI for API testing
    }

    app.UseHttpsRedirection();  // Enforce HTTPS
    app.UseAuthorization();     // Enable authorization
    app.MapControllers();       // Map API controllers

    app.Run();   // Run the web app
}
catch (Exception ex)
{
    // Log any exceptions during application startup
    logger.Error(ex, "Application stopped due to an exception.");
    throw;  // Re-throw the exception to stop the application
}
finally
{
    // Log application shutdown
    logger.Info("Application has stopped.");
    LogManager.Shutdown();  // Properly shutdown NLog
}
