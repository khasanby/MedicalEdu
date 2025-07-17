namespace MedicalEdu.Application.Common.Configuration;

/// <summary>
/// Configuration options for cache invalidation behavior.
/// </summary>
public class CacheInvalidationOptions
{
    /// <summary>
    /// Gets or sets whether to require explicit cache invalidation attributes.
    /// When true, commands without attributes will throw an exception instead of clearing all cache.
    /// Default is false (clears all cache as fallback).
    /// </summary>
    public bool RequireExplicitAttributes { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to throw exceptions in development/QA environments when no attributes are found.
    /// This helps catch missing cache invalidation configuration early.
    /// Default is true.
    /// </summary>
    public bool ThrowOnMissingAttributesInDevelopment { get; set; } = true;

    /// <summary>
    /// Gets or sets the environment name where strict validation should be applied.
    /// Default is "Development". Note: This is used as a fallback when IHostEnvironment.IsDevelopment() is not available.
    /// </summary>
    public string StrictValidationEnvironment { get; set; } = "Development";
} 