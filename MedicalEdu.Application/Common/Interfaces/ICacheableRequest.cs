using MediatR;

namespace MedicalEdu.Application.Common.Interfaces;

public interface ICacheableRequest
{
    /// <summary>
    /// Gets the cache duration for this request.
    /// </summary>
    public TimeSpan CacheDuration { get; }

    /// <summary>
    /// Gets a custom cache key for this request. If not implemented, a default key will be generated.
    /// </summary>
    /// <returns>The custom cache key, or null to use the default key generation.</returns>
    string? GetCacheKey() => null;
}

/// <summary>
/// Generic marker interface for requests that should be cached.
/// </summary>
public interface ICacheableRequest<TResponse> : IRequest<TResponse>, ICacheableRequest
{
}