using System.Text.Json;
using System.Text.RegularExpressions;

namespace MedicalEdu.Domain.ValueObjects;

public sealed class RecurringSchedule : IEquatable<RecurringSchedule>
{
    private static readonly Regex CronPattern = new(
        @"^(\*|([0-9]|1[0-9]|2[0-9]|3[0-9]|4[0-9]|5[0-9])|\*\/([0-9]|1[0-9]|2[0-9]|3[0-9]|4[0-9]|5[0-9])) (\*|([0-9]|1[0-9]|2[0-3])|\*\/([0-9]|1[0-9]|2[0-3])) (\*|([1-9]|1[0-9]|2[0-9]|3[0-1])|\*\/([1-9]|1[0-9]|2[0-9]|3[0-1])) (\*|([1-9]|1[0-2])|\*\/([1-9]|1[0-2])) (\*|([0-6])|\*\/([0-6]))$",
        RegexOptions.Compiled);

    /// <summary>
    /// Gets the schedule expression (CRON or JSON).
    /// </summary>
    public string Expression { get; }

    /// <summary>
    /// Gets the type of schedule.
    /// </summary>
    public ScheduleType Type { get; }

    private RecurringSchedule(string expression, ScheduleType type)
    {
        Expression = expression;
        Type = type;
    }

    /// <summary>
    /// Creates a new RecurringSchedule instance from a CRON expression.
    /// </summary>
    public static RecurringSchedule CreateCron(string cronExpression)
    {
        if (string.IsNullOrWhiteSpace(cronExpression))
            throw new ArgumentException("CRON expression cannot be empty", nameof(cronExpression));

        if (!IsValidCronExpression(cronExpression))
            throw new ArgumentException("Invalid CRON expression format", nameof(cronExpression));

        return new RecurringSchedule(cronExpression, ScheduleType.Cron);
    }

    /// <summary>
    /// Creates a new RecurringSchedule instance from a JSON configuration.
    /// </summary>
    public static RecurringSchedule CreateJson(string jsonConfig)
    {
        if (string.IsNullOrWhiteSpace(jsonConfig))
            throw new ArgumentException("JSON configuration cannot be empty", nameof(jsonConfig));

        if (!IsValidJsonConfig(jsonConfig))
            throw new ArgumentException("Invalid JSON configuration format", nameof(jsonConfig));

        return new RecurringSchedule(jsonConfig, ScheduleType.Json);
    }

    /// <summary>
    /// Attempts to create a RecurringSchedule instance from a CRON expression.
    /// </summary>
    public static bool TryCreateCron(string cronExpression, out RecurringSchedule? result)
    {
        try
        {
            result = CreateCron(cronExpression);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    /// <summary>
    /// Attempts to create a RecurringSchedule instance from a JSON configuration.
    /// </summary>
    public static bool TryCreateJson(string jsonConfig, out RecurringSchedule? result)
    {
        try
        {
            result = CreateJson(jsonConfig);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    private static bool IsValidCronExpression(string cronExpression)
    {
        return CronPattern.IsMatch(cronExpression.Trim());
    }

    private static bool IsValidJsonConfig(string jsonConfig)
    {
        try
        {
            var document = JsonDocument.Parse(jsonConfig);
            var root = document.RootElement;

            // Basic validation - check for required fields
            return root.TryGetProperty("frequency", out _) &&
                   root.TryGetProperty("interval", out _);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the next occurrence based on the schedule.
    /// </summary>
    public DateTime GetNextOccurrence(DateTime fromDate)
    {
        // In a real application, you would use a proper CRON parser library
        return fromDate.AddDays(1);
    }

    /// <summary>
    /// Implicitly converts the schedule to a string.
    /// </summary>
    public static implicit operator string(RecurringSchedule schedule) => schedule.Expression;

    /// <summary>
    /// Returns the string representation of the schedule.
    /// </summary>
    public override string ToString() => Expression;

    public bool Equals(RecurringSchedule? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Expression == other.Expression && Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as RecurringSchedule);
    }

    /// <summary>
    /// Returns the hash code for the schedule.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Expression, Type);
    }

    /// <summary>
    /// Checks if two schedules are equal.
    /// </summary>
    public static bool operator ==(RecurringSchedule? left, RecurringSchedule? right)
    {
        return EqualityComparer<RecurringSchedule>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two schedules are not equal.
    /// </summary>
    public static bool operator !=(RecurringSchedule? left, RecurringSchedule? right)
    {
        return !(left == right);
    }
}

/// <summary>
/// Represents the type of recurring schedule.
/// </summary>
public enum ScheduleType
{
    /// <summary>
    /// CRON expression format.
    /// </summary>
    Cron,

    /// <summary>
    /// JSON configuration format.
    /// </summary>
    Json
}