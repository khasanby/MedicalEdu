using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using MedicalEdu.Infrastructure.DataAccess.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;

namespace MedicalEdu.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var hostBuilder = WebApplication.CreateBuilder(args);

        SetupApplicationComponents(hostBuilder);
        SetupMiddlewarePipeline(hostBuilder);

        var webApp = hostBuilder.Build();

        await InitializeApplicationAsync(webApp, hostBuilder, default);

        var logger = webApp.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation(
            "System Info - CPU Cores: {coreCount}, Server GC: {serverGc}, LOH Mode: {lohMode}, GC Latency: {latencyMode}.",
            Environment.ProcessorCount,
            System.Runtime.GCSettings.IsServerGC,
            System.Runtime.GCSettings.LargeObjectHeapCompactionMode,
            System.Runtime.GCSettings.LatencyMode);

        await webApp.RunAsync();
    }

    private static void SetupApplicationComponents(WebApplicationBuilder hostBuilder)
    {
        SetupDataCompression(hostBuilder);
        SetupConfigurationSources(hostBuilder);
        SetupApplicationLogging(hostBuilder);
        SetupApplicationServices(hostBuilder);
        SetupApplicationSecurity(hostBuilder);
    }

    private static void SetupDataCompression(WebApplicationBuilder hostBuilder)
    {
        hostBuilder.Services.AddRequestDecompression();
        hostBuilder.Services.AddResponseCompression(compressionOptions =>
        {
            compressionOptions.EnableForHttps = true;
            compressionOptions.Providers.Add<BrotliCompressionProvider>();
            compressionOptions.Providers.Add<GzipCompressionProvider>();
        });

        hostBuilder.Services.Configure<BrotliCompressionProviderOptions>(brotliOptions =>
        {
            brotliOptions.Level = CompressionLevel.Fastest;
        });

        hostBuilder.Services.Configure<GzipCompressionProviderOptions>(gzipOptions =>
        {
            gzipOptions.Level = CompressionLevel.SmallestSize;
        });
    }

    private static void SetupConfigurationSources(WebApplicationBuilder hostBuilder)
    {
        hostBuilder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{hostBuilder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    private static void SetupApplicationServices(WebApplicationBuilder hostBuilder)
    {
        // Register infrastructure components
        hostBuilder.Services.AddInfrastructure(hostBuilder.Configuration);

        hostBuilder.Services.AddControllers().AddJsonOptions(jsonOptions =>
        {
            jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            jsonOptions.JsonSerializerOptions.WriteIndented = false;
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        hostBuilder.Services.AddCors();

        hostBuilder.Services.AddEndpointsApiExplorer();
        hostBuilder.Services.AddSwaggerGen(swaggerOptions =>
        {
            swaggerOptions.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MedicalEdu API",
                Version = "v1",
                Description = "Medical Education Platform API"
            });
        });
    }

    private static void SetupApplicationSecurity(WebApplicationBuilder hostBuilder)
    {
        // TODO: Implement authentication and authorization
        // Example implementation:
        // hostBuilder.Services.AddMicrosoftIdentityWebApiAuthentication(hostBuilder.Configuration);
        // hostBuilder.Services.AddAuthorizationBuilder()
        //     .AddPolicy("AdminAccess", policy =>
        //     {
        //         policy.RequireAuthenticatedUser();
        //         policy.RequireRole("Administrator");
        //     });
    }

    private static void SetupApplicationLogging(WebApplicationBuilder hostBuilder)
    {
        // TODO: Configure logging framework
        // Example with Serilog:
        // Log.Logger = new LoggerConfiguration().CreateLogger();
        // hostBuilder.Host.UseSerilog((context, loggerConfig) =>
        //     loggerConfig.ReadFrom.Configuration(context.Configuration)
        //     .Enrich.FromLogContext()
        //     .Enrich.WithCorrelationId());
    }

    private static void SetupMiddlewarePipeline(WebApplicationBuilder hostBuilder)
    {
        // This method is intentionally left empty as middleware setup is handled in InitializeApplicationAsync
        // This separation allows for better organization and potential future middleware configuration
    }

    private static async ValueTask InitializeApplicationAsync(WebApplication webApp, WebApplicationBuilder hostBuilder, CancellationToken cancellationToken)
    {
        // TODO: Add application initialization logic here
        // Example: await webApp.Services.InitializeServicesAsync(cancellationToken);

        webApp.UseRequestDecompression();
        webApp.UseResponseCompression();

        if (hostBuilder.Environment.IsDevelopment())
        {
            webApp.UseDeveloperExceptionPage();

            webApp.UseSwagger();
            webApp.UseSwaggerUI(swaggerUiOptions =>
            {
                swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "MedicalEdu API v1");
            });
        }

        webApp.UseExceptionHandler("/error");
        webApp.UseRouting();

        webApp.UseAuthentication();
        webApp.UseAuthorization();

        webApp.UseCors(corsPolicyBuilder => corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .Build());

        webApp.MapControllers();
    }
}
