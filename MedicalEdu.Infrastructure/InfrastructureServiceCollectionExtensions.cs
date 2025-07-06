using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MedicalEdu.Infrastructure.DataAccess;
using MedicalEdu.Infrastructure.DataAccess.Repositories;
using MedicalEdu.Domain.DataAccess;
using MedicalEdu.Domain.DataAccess.Repositories;
using MedicalEdu.Domain.DataAccess.Services;

namespace MedicalEdu.Infrastructure;

/// <summary>
/// Extension methods for registering infrastructure services in the DI container.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<MedicalEduDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Default"),
                b => b.MigrationsAssembly("MedicalEdu.Migrations")));

        // Register interface abstraction
        services.AddScoped<IMedicalEduDbContext>(provider => provider.GetRequiredService<MedicalEduDbContext>());

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IAvailabilitySlotRepository, AvailabilitySlotRepository>();

        // Register application services
        services.AddScoped<IUserService, UserService>();

        return services;
    }
} 