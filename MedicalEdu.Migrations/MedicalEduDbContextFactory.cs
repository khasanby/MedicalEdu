using MedicalEdu.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MedicalEdu.Migrations;

/// <summary>
/// Design-time factory for MedicalEduDbContext to enable migrations generation.
/// </summary>
public class MedicalEduDbContextFactory : IDesignTimeDbContextFactory<MedicalEduDbContext>
{
    public MedicalEduDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("MedDB");

        var optionsBuilder = new DbContextOptionsBuilder<MedicalEduDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new MedicalEduDbContext(optionsBuilder.Options);
    }
}