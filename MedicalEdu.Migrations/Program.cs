using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MedicalEdu.Migrations;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("MedicalEdu.Migrations: STARTED.");

        try
        {
            // Setup configuration
            var configuration = BuildConfiguration();

            // Setup logging
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            var logger = loggerFactory.CreateLogger<Program>();

            // Create database context
            var contextFactory = new MedicalEduDbContextFactory();
            using var context = contextFactory.CreateDbContext(args);

            logger.LogInformation("Connecting to database...");

            // Check if database exists
            var databaseExists = await context.Database.CanConnectAsync();
            if (!databaseExists)
            {
                logger.LogInformation("Database does not exist. Creating database...");
                await context.Database.EnsureCreatedAsync();
                logger.LogInformation("Database created successfully.");
            }

            // Apply pending migrations
            logger.LogInformation("Applying pending migrations...");
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                logger.LogInformation("Found {Count} pending migrations: {Migrations}",
                    pendingMigrations.Count(), string.Join(", ", pendingMigrations));

                await context.Database.MigrateAsync();
                logger.LogInformation("All migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("No pending migrations found. Database is up to date.");
            }

            // Verify database is up to date
            var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
            logger.LogInformation("Applied migrations: {Count}", appliedMigrations.Count());

            Console.WriteLine("MedicalEdu.Migrations: SUCCESS.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MedicalEdu.Migrations: FAILED.");
            Console.WriteLine($"Error: {ex.Message}");

            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Error: {ex.InnerException.Message}");
            }

            Environment.Exit(1);
        }
    }

    private static IConfiguration BuildConfiguration()
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
}