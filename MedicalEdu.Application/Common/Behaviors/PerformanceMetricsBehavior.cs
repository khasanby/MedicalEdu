using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MedicalEdu.Application.Common.Behaviors;

public sealed class PerformanceMetricsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceMetricsBehavior<TRequest, TResponse>> _logger;

    public PerformanceMetricsBehavior(ILogger<PerformanceMetricsBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestType = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Starting {RequestType} at {Timestamp}", requestType, DateTime.UtcNow);

        try
        {
            var response = await next();
            stopwatch.Stop();

            _logger.LogInformation(
                "{RequestType} completed successfully in {ElapsedMilliseconds}ms",
                requestType,
                stopwatch.ElapsedMilliseconds);

            // Log performance metrics based on duration
            if (stopwatch.ElapsedMilliseconds > 1000)
            {
                _logger.LogWarning(
                    "{RequestType} took {ElapsedMilliseconds}ms - this is slower than expected",
                    requestType,
                    stopwatch.ElapsedMilliseconds);
            }
            else if (stopwatch.ElapsedMilliseconds > 500)
            {
                _logger.LogInformation(
                    "{RequestType} took {ElapsedMilliseconds}ms - moderate performance",
                    requestType,
                    stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogDebug(
                    "{RequestType} took {ElapsedMilliseconds}ms - good performance",
                    requestType,
                    stopwatch.ElapsedMilliseconds);
            }

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "{RequestType} failed after {ElapsedMilliseconds}ms",
                requestType,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}