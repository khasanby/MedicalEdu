using FluentValidation;
using MediatR;
using MedicalEdu.Application.Common.Behaviors;
using MedicalEdu.Application.Common.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalEdu.Application.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add MediatR with assembly scanning
        services.AddMediatR(cfg => 
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        // Add FluentValidation with assembly scanning
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Add Memory Cache for CachingBehavior
        services.AddMemoryCache();
        
        // Register cache service
        services.AddSingleton<ICacheService, MemoryCacheService>();

        // Configure cache invalidation options
        services.Configure<CacheInvalidationOptions>(options =>
        {
            // Default configuration - can be overridden in appsettings.json
            options.RequireExplicitAttributes = false;
            options.ThrowOnMissingAttributesInDevelopment = true;
            options.StrictValidationEnvironment = "Development"; // Fallback for environments where IsDevelopment() is not available
        });

        // Add pipeline behaviors in order of execution
        // Order matters: Validation -> Caching -> Performance -> Transaction -> Cache Invalidation -> Logging
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceMetricsBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        // Add AutoMapper
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        return services;
    }
}