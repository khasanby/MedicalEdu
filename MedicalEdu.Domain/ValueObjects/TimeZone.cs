using System;

namespace MedicalEdu.Domain.ValueObjects;

public sealed class TimeZoneId : IEquatable<TimeZoneId>
{
    /// <summary>
    /// Gets the time zone identifier.
    /// </summary>
    public string Id { get; }

    private TimeZoneId(string id)
    {
        Id = id;
    }

    public static TimeZoneId Create(string timeZoneId)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
            throw new ArgumentException("Time zone identifier cannot be empty", nameof(timeZoneId));

        if (!IsValidTimeZone(timeZoneId))
            throw new ArgumentException("Invalid time zone identifier", nameof(timeZoneId));

        return new TimeZoneId(timeZoneId);
    }

    public static TimeZoneId Parse(string timeZoneId)
    {
        return Create(timeZoneId);
    }

    private static bool IsValidTimeZone(string timeZoneId)
    {
        try
        {
            // Try to get the time zone info to validate
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return timeZoneInfo != null;
        }
        catch
        {
            // If FindSystemTimeZoneById fails, check if it's a common time zone
            return IsCommonTimeZone(timeZoneId);
        }
    }

    private static bool IsCommonTimeZone(string timeZoneId)
    {
        var commonTimeZones = new[]
        {
            "UTC", "GMT", "EST", "EDT", "CST", "CDT", "MST", "MDT", "PST", "PDT",
            "Europe/London", "Europe/Paris", "Europe/Berlin", "Europe/Moscow",
            "America/New_York", "America/Chicago", "America/Denver", "America/Los_Angeles",
            "Asia/Tokyo", "Asia/Shanghai", "Asia/Kolkata", "Australia/Sydney"
        };

        return commonTimeZones.Contains(timeZoneId, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Implicitly converts the time zone to a string.
    /// </summary>
    public static implicit operator string(TimeZoneId timeZone) => timeZone.Id;

    /// <summary>
    /// Returns the string representation of the time zone.
    /// </summary>
    public override string ToString() => Id;

    public bool Equals(TimeZoneId? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TimeZoneId);
    }

    /// <summary>
    /// Returns the hash code for the time zone.
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Checks if two time zones are equal.
    /// </summary>
    public static bool operator ==(TimeZoneId? left, TimeZoneId? right)
    {
        return EqualityComparer<TimeZoneId>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two time zones are not equal.
    /// </summary>
    public static bool operator !=(TimeZoneId? left, TimeZoneId? right)
    {
        return !(left == right);
    }
} 