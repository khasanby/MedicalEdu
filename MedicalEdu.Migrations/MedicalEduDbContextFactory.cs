using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MedicalEdu.Infrastructure.DataAccess;
using MedicalEdu.Domain.DataAccess;

namespace MedicalEdu.Migrations;

/// <summary>
/// Design-time factory for MedicalEduDbContext to enable migrations generation.
/// </summary>
public class MedicalEduDbContextFactory : IDesignTimeDbContextFactory<MedicalEduDbContext>
{
    public MedicalEduDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MedicalEduDbContext>();
        
        // Use a default connection string for design-time operations
        // This will be overridden at runtime with the actual connection string
        optionsBuilder.UseNpgsql("Host=localhost;Database=MedicalEdu;Username=postgres;Password=postgres");
        
        return new MedicalEduDbContext(optionsBuilder.Options);
    }
} 