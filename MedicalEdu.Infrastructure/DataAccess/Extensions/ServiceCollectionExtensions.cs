using MedicalEdu.Domain.DataAccess;
using MedicalEdu.Domain.DataAccess.Repositories;
using MedicalEdu.Infrastructure.DataAccess.Interceptors;
using MedicalEdu.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalEdu.Infrastructure.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<MedicalEduDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            });

            // Add interceptors
            options.AddInterceptors(new AuditSaveChangesInterceptor());
        });

        // Register DbContext interface
        services.AddScoped<IMedicalEduDbContext>(provider =>
            provider.GetRequiredService<MedicalEduDbContext>());

        // Register repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // TODO: Add other repositories as they are implemented
        // services.AddScoped<ICourseRepository, CourseRepository>();
        // services.AddScoped<IBookingRepository, BookingRepository>();
        // services.AddScoped<IAvailabilitySlotRepository, AvailabilitySlotRepository>();
        // services.AddScoped<IPaymentRepository, PaymentRepository>();
        // services.AddScoped<IPromoCodeRepository, PromoCodeRepository>();
        // services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }
}