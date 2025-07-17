using MediatR;
using MedicalEdu.Application.Common.Caching;
using MedicalEdu.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MedicalEdu.Application.Common.Behaviors;

public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    // Deterministic JSON serialization options
    private static readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public CachingBehavior(
        ICacheService cache,
        ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Only cache requests that explicitly implement ICacheableRequest
        if (request is not ICacheableRequest cacheable)
        {
            _logger.LogDebug("Request {RequestType} is not cacheable, skipping cache", typeof(TRequest).Name);
            return await next();
        }

        var cacheKey = GetCacheKey(request, cacheable);
        var ttl = cacheable.CacheDuration;

        _logger.LogDebug("Checking cache for {RequestType} with key {CacheKey}", typeof(TRequest).Name, cacheKey);

        // Check if value exists in cache
        if (_cache.TryGetValue(cacheKey, out TResponse? cachedResponse))
        {
            _logger.LogDebug("Cache hit for {RequestType} with key {CacheKey}", typeof(TRequest).Name, cacheKey);
            return CloneIfNeeded(cachedResponse!);
        }

        _logger.LogDebug("Cache miss for {RequestType} with key {CacheKey}, executing handler", typeof(TRequest).Name, cacheKey);

        // Use GetOrCreateAsync to prevent duplicate handler execution on parallel requests
        var cacheOptions = new CacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl,
            SlidingExpiration = TimeSpan.FromMinutes(5)
        };

        var response = await _cache.GetOrCreateAsync(cacheKey, async () =>
        {
            _logger.LogDebug("Executing handler for {RequestType} with key {CacheKey}", typeof(TRequest).Name, cacheKey);
            return await next();
        }, cacheOptions);

        _logger.LogDebug("Cached response for {RequestType} with key {CacheKey} for {CacheDuration}",
            typeof(TRequest).Name, cacheKey, ttl);

        return CloneIfNeeded(response);
    }

    private static string GetCacheKey(TRequest request, ICacheableRequest cacheable)
    {
        // Use custom cache key if provided, otherwise generate default
        var customKey = cacheable.GetCacheKey();
        if (!string.IsNullOrEmpty(customKey))
        {
            return customKey;
        }

        return GenerateDefaultCacheKey(request);
    }

    private static string GenerateDefaultCacheKey(TRequest request)
    {
        // Use SHA256 for better performance and security
        using var sha = SHA256.Create();
        var requestJson = JsonSerializer.Serialize(request, _serializerOptions);
        var bytes = Encoding.UTF8.GetBytes(requestJson);
        var hash = sha.ComputeHash(bytes);
        var hashString = Convert.ToHexString(hash);

        return $"{request.GetType().Name}_{hashString}";
    }

    private static TResponse CloneIfNeeded(TResponse response)
    {
        // For now, return the response as-is
        // In a production environment, you might want to implement deep cloning
        // for mutable objects to prevent cache pollution
        return response;
    }
}