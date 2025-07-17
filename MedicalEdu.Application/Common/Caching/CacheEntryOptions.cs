namespace MedicalEdu.Application.Common.Caching;

/// <summary>
/// Options for cache entry configuration.
/// </summary>
public sealed class CacheEntryOptions
{
    /// <summary>
    /// Gets or sets the absolute expiration time relative to now.
    /// </summary>
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    /// <summary>
    /// Gets or sets the sliding expiration time.
    /// </summary>
    public TimeSpan? SlidingExpiration { get; set; }

    /// <summary>
    /// Gets or sets the priority for eviction.
    /// </summary>
    public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;

    /// <summary>
    /// Gets or sets the size of the cache entry.
    /// </summary>
    public long? Size { get; set; }
}

/// <summary>
/// Priority for cache item eviction.
/// </summary>
public enum CacheItemPriority
{
    /// <summary>
    /// Low priority - first to be evicted.
    /// </summary>
    Low = 0,

    /// <summary>
    /// Normal priority.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// High priority - last to be evicted.
    /// </summary>
    High = 2,

    /// <summary>
    /// Never evict - only removed when explicitly removed or expired.
    /// </summary>
    NeverRemove = 3
} 