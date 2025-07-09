using MedicalEdu.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MedicalEdu.Migrations;

public sealed class MedicalEduDbContextFactory : IDesignTimeDbContextFactory<MedicalEduDbContext>
{
    public MedicalEduDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = GetConnectionString(configuration);

        var optionsBuilder = new DbContextOptionsBuilder<MedicalEduDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        });

        if (IsRuntimeExecution())
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }

        return new MedicalEduDbContext(optionsBuilder.Options);
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

    private static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                               configuration.GetConnectionString("Default") ??
                               configuration.GetConnectionString("MedDB") ??
                               throw new InvalidOperationException("No connection string found. Please configure one of: DefaultConnection, Default, or MedDB");

        return connectionString;
    }

    private static bool IsRuntimeExecution()
    {
        return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")) ||
               Environment.GetCommandLineArgs().Any(arg => arg.Contains("MedicalEdu.Migrations"));
    }
}