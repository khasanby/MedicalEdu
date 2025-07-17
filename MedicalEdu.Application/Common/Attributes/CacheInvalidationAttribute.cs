namespace MedicalEdu.Application.Common.Attributes;


[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class CacheInvalidationAttribute : Attribute
{
    /// <summary>
    /// Gets the cache prefixes to invalidate.
    /// </summary>
    public IReadOnlyList<string> CachePrefixes { get; }

    /// <summary>
    /// Gets the reason for invalidation (for documentation purposes).
    /// </summary>
    public string Reason { get; }

    /// <summary>
    /// Initializes a new instance of the CacheInvalidationAttribute with a single cache prefix.
    /// </summary>
    public CacheInvalidationAttribute(string cachePrefix, string reason = "")
    {
        CachePrefixes = new[] { cachePrefix ?? throw new ArgumentNullException(nameof(cachePrefix)) };
        Reason = reason ?? string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the CacheInvalidationAttribute with multiple cache prefixes.
    /// </summary>
    public CacheInvalidationAttribute(string[] cachePrefixes, string reason = "")
    {
        CachePrefixes = cachePrefixes ?? throw new ArgumentNullException(nameof(cachePrefixes));
        if (cachePrefixes.Length == 0)
            throw new ArgumentException("At least one cache prefix must be specified.", nameof(cachePrefixes));
        if (cachePrefixes.Any(string.IsNullOrEmpty))
            throw new ArgumentException("Cache prefixes cannot be null or empty.", nameof(cachePrefixes));

        Reason = reason ?? string.Empty;
    }
}

/// <summary>
/// Predefined cache prefixes for common entities.
/// </summary>
public static class CachePrefixes
{
    // Course-related prefixes
    public const string GetAllCourses = "GetAllCourses";
    public const string GetCourseById = "GetCourseById";
    public const string GetCoursesByInstructor = "GetCoursesByInstructor";
    public const string GetCoursesByCategory = "GetCoursesByCategory";

    // User-related prefixes
    public const string GetAllUsers = "GetAllUsers";
    public const string GetUserById = "GetUserById";
    public const string GetUsersByRole = "GetUsersByRole";

    // Enrollment-related prefixes
    public const string GetEnrollments = "GetEnrollments";
    public const string GetEnrollmentsByUser = "GetEnrollmentsByUser";
    public const string GetEnrollmentsByCourse = "GetEnrollmentsByCourse";

    // Booking-related prefixes
    public const string GetBookings = "GetBookings";
    public const string GetBookingsByUser = "GetBookingsByUser";
    public const string GetBookingsByInstructor = "GetBookingsByInstructor";

    // Availability-related prefixes
    public const string GetAvailabilitySlots = "GetAvailabilitySlots";
    public const string GetAvailabilitySlotsByInstructor = "GetAvailabilitySlotsByInstructor";

    // Payment-related prefixes
    public const string GetPayments = "GetPayments";
    public const string GetPaymentsByUser = "GetPaymentsByUser";

    // Rating-related prefixes
    public const string GetCourseRatings = "GetCourseRatings";
    public const string GetInstructorRatings = "GetInstructorRatings";

    // Notification-related prefixes
    public const string GetNotifications = "GetNotifications";
    public const string GetNotificationsByUser = "GetNotificationsByUser";
}