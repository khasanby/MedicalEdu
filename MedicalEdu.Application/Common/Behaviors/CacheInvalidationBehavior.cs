using MediatR;
using MedicalEdu.Application.Common.Attributes;
using MedicalEdu.Application.Common.Caching;
using MedicalEdu.Application.Common.Configuration;
using MedicalEdu.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Reflection;

namespace MedicalEdu.Application.Common.Behaviors;

public sealed class CacheInvalidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;
    private readonly ILogger<CacheInvalidationBehavior<TRequest, TResponse>> _logger;
    private readonly IHostEnvironment _environment;
    private readonly CacheInvalidationOptions _options;
    private static readonly ConcurrentDictionary<Type, CacheInvalidationAttribute[]> _metadataCache = new();

    public CacheInvalidationBehavior(
        ICacheService cache,
        ILogger<CacheInvalidationBehavior<TRequest, TResponse>> logger,
        IHostEnvironment environment,
        IOptions<CacheInvalidationOptions> options)
    {
        _cache = cache;
        _logger = logger;
        _environment = environment;
        _options = options.Value;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Only invalidate cache for commands (write operations)
        if (request is not ICommand<TResponse>)
        {
            _logger.LogDebug("Request {RequestType} is not a command, skipping cache invalidation", typeof(TRequest).Name);
            return await next();
        }

        var type = request.GetType().Name;
        _logger.LogDebug("{Type} → invalidating caches", type);

        var response = await next();

        // Invalidate cache entries based on metadata (async to support distributed caches)
        await InvalidateRelatedCacheAsync(request, cancellationToken);

        _logger.LogDebug("{Type} → cache invalidation complete", type);

        return response;
    }

    private async Task InvalidateRelatedCacheAsync(TRequest request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();

        // Cache attribute lookup to avoid repeated reflection on hot code paths
        if (!_metadataCache.TryGetValue(requestType, out var invalidationAttributes))
        {
            invalidationAttributes = requestType.GetCustomAttributes<CacheInvalidationAttribute>(inherit: false).ToArray();
            _metadataCache[requestType] = invalidationAttributes;
        }

        if (!invalidationAttributes.Any())
        {
            var shouldThrow = _options.RequireExplicitAttributes ||(_options.ThrowOnMissingAttributesInDevelopment && _environment.IsDevelopment());
            if (shouldThrow)
            {
                var message = $"Command {requestType.Name} is missing cache invalidation attributes. Add [CacheInvalidation] attributes to specify which cache prefixes to invalidate.";
                _logger.LogError(message);
                throw new InvalidOperationException(message);
            }

            _logger.LogWarning("No cache invalidation attributes found for command {RequestType}, invalidating all cache entries", requestType.Name);
            await _cache.ClearAsync(cancellationToken);
            return;
        }

        var invalidatedPrefixes = new List<string>();

        foreach (var attribute in invalidationAttributes)
        {
            foreach (var prefix in attribute.CachePrefixes)
            {
                await _cache.RemoveByPrefixAsync(prefix, cancellationToken);
                invalidatedPrefixes.Add(prefix);
            }

            // Batch log per attribute to reduce log noise in high-throughput scenarios
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Invalidated {Count} prefixes: {Prefixes} → {Reason}",
                    attribute.CachePrefixes.Count, string.Join(", ", attribute.CachePrefixes), attribute.Reason);
            }
        }

        _logger.LogInformation("Invalidated {Count} cache prefixes for {RequestType}: {Prefixes}",
            invalidatedPrefixes.Count, requestType.Name, string.Join(", ", invalidatedPrefixes));
    }
}