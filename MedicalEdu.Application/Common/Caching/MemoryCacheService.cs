using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace MedicalEdu.Application.Common.Caching;

/// <summary>
/// Memory cache service implementation that supports prefix-based invalidation.
/// </summary>
public sealed class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheService> _logger;
    private readonly ConcurrentDictionary<string, HashSet<string>> _prefixRegistry;

    public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
        _prefixRegistry = new ConcurrentDictionary<string, HashSet<string>>();
    }

    public bool TryGetValue<T>(string key, out T? value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public void Set<T>(string key, T value, CacheEntryOptions options)
    {
        // Register the key with its prefix for later invalidation
        RegisterKey(key);

        var memoryCacheOptions = new MemoryCacheEntryOptions();

        if (options.AbsoluteExpirationRelativeToNow.HasValue)
            memoryCacheOptions.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;

        if (options.SlidingExpiration.HasValue)
            memoryCacheOptions.SlidingExpiration = options.SlidingExpiration;

        memoryCacheOptions.Priority = (Microsoft.Extensions.Caching.Memory.CacheItemPriority)options.Priority;

        if (options.Size.HasValue)
            memoryCacheOptions.Size = options.Size;

        _cache.Set(key, value, memoryCacheOptions);

        _logger.LogDebug("Cached value for key {Key}", key);
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, CacheEntryOptions options)
    {
        // Register the key with its prefix for later invalidation
        RegisterKey(key);

        var memoryCacheOptions = new MemoryCacheEntryOptions();

        if (options.AbsoluteExpirationRelativeToNow.HasValue)
            memoryCacheOptions.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;

        if (options.SlidingExpiration.HasValue)
            memoryCacheOptions.SlidingExpiration = options.SlidingExpiration;

        memoryCacheOptions.Priority = (Microsoft.Extensions.Caching.Memory.CacheItemPriority)options.Priority;

        if (options.Size.HasValue)
            memoryCacheOptions.Size = options.Size;

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = memoryCacheOptions.AbsoluteExpirationRelativeToNow;
            entry.SlidingExpiration = memoryCacheOptions.SlidingExpiration;
            entry.Priority = memoryCacheOptions.Priority;
            entry.Size = memoryCacheOptions.Size;

            return await factory();
        });
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
        UnregisterKey(key);

        _logger.LogDebug("Removed cache entry for key {Key}", key);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Remove(key);
        return Task.CompletedTask;
    }

    public void RemoveByPrefix(string prefix)
    {
        if (_prefixRegistry.TryGetValue(prefix, out var keys))
        {
            var keysToRemove = keys.ToList(); // Create a copy to avoid modification during enumeration

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
                UnregisterKey(key);
            }

            _logger.LogDebug("Removed {Count} cache entries with prefix {Prefix}", keysToRemove.Count, prefix);
        }
        else
        {
            _logger.LogDebug("No cache entries found with prefix {Prefix}", prefix);
        }
    }

    public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (_prefixRegistry.TryGetValue(prefix, out var keys))
        {
            var keysToRemove = keys.ToList(); // Create a copy to avoid modification during enumeration

            foreach (var key in keysToRemove)
            {
                cancellationToken.ThrowIfCancellationRequested();
                _cache.Remove(key);
                UnregisterKey(key);
            }

            _logger.LogDebug("Removed {Count} cache entries with prefix {Prefix}", keysToRemove.Count, prefix);
        }
        else
        {
            _logger.LogDebug("No cache entries found with prefix {Prefix}", prefix);
        }
        
        return Task.CompletedTask;
    }

    public void Clear()
    {
        // Clear all registered keys
        var allKeys = _prefixRegistry.Values.SelectMany(keys => keys).ToList();

        foreach (var key in allKeys)
        {
            _cache.Remove(key);
        }

        _prefixRegistry.Clear();

        _logger.LogDebug("Cleared all cache entries");
    }

    public Task ClearAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // Clear all registered keys
        var allKeys = _prefixRegistry.Values.SelectMany(keys => keys).ToList();

        foreach (var key in allKeys)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _cache.Remove(key);
        }

        _prefixRegistry.Clear();

        _logger.LogDebug("Cleared all cache entries");
        
        return Task.CompletedTask;
    }

    private void RegisterKey(string key)
    {
        // Extract prefix from key (e.g., "GetAllCourses_" -> "GetAllCourses")
        var prefix = ExtractPrefix(key);

        _prefixRegistry.AddOrUpdate(
            prefix,
            new HashSet<string> { key },
            (existingPrefix, existingKeys) =>
            {
                existingKeys.Add(key);
                return existingKeys;
            });
    }

    private void UnregisterKey(string key)
    {
        var prefix = ExtractPrefix(key);

        if (_prefixRegistry.TryGetValue(prefix, out var keys))
        {
            keys.Remove(key);

            // Remove the prefix entry if it's empty
            if (keys.Count == 0)
            {
                _prefixRegistry.TryRemove(prefix, out _);
            }
        }
    }

    private static string ExtractPrefix(string key)
    {
        // Extract the prefix from the key (everything before the last underscore)
        var lastUnderscoreIndex = key.LastIndexOf('_');
        return lastUnderscoreIndex > 0 ? key[..lastUnderscoreIndex] : key;
    }
}