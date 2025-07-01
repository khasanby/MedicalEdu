namespace MedicalEdu.Domain.ValueObjects;

public sealed class TimeSlot : IEquatable<TimeSlot>
{
    /// <summary>
    /// Gets the start time of the time slot.
    /// </summary>
    public DateTime StartTime { get; }

    /// <summary>
    /// Gets the end time of the time slot.
    /// </summary>
    public DateTime EndTime { get; }

    /// <summary>
    /// Gets the duration of the time slot.
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

    private TimeSlot(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    public static TimeSlot Create(DateTime startTime, DateTime endTime)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time");

        if (startTime < DateTime.UtcNow)
            throw new ArgumentException("Start time cannot be in the past");

        if (Duration > TimeSpan.FromHours(8))
            throw new ArgumentException("Time slot cannot exceed 8 hours");

        return new TimeSlot(startTime, endTime);
    }

    public static TimeSlot Create(DateTime startTime, TimeSpan duration)
    {
        return Create(startTime, startTime.Add(duration));
    }

    public bool OverlapsWith(TimeSlot other)
    {
        return StartTime < other.EndTime && EndTime > other.StartTime;
    }

    public bool IsInPast => EndTime < DateTime.UtcNow;
    public bool IsInFuture => StartTime > DateTime.UtcNow;
    public bool IsCurrentlyActive => StartTime <= DateTime.UtcNow && EndTime > DateTime.UtcNow;

    public TimeSlot Extend(TimeSpan additionalTime)
    {
        return Create(StartTime, EndTime.Add(additionalTime));
    }

    public TimeSlot Shorten(TimeSpan reduction)
    {
        if (reduction >= Duration)
            throw new ArgumentException("Reduction cannot be greater than or equal to current duration");

        return Create(StartTime, EndTime.Subtract(reduction));
    }

    /// <summary>
    /// Returns the string representation of the time slot.
    /// </summary>
    public override string ToString() => $"{StartTime:yyyy-MM-dd HH:mm} - {EndTime:HH:mm}";

    /// <summary>
    /// Checks if the time slot is equal to another time slot.
    /// </summary>
    public bool Equals(TimeSlot? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return StartTime == other.StartTime && EndTime == other.EndTime;
    }

    /// <summary>
    /// Checks if the time slot is equal to another time slot.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return Equals(obj as TimeSlot);
    }

    /// <summary>
    /// Returns the hash code for the time slot.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(StartTime, EndTime);
    }

    /// <summary>
    /// Checks if two time slots are equal.
    /// </summary>
    public static bool operator ==(TimeSlot? left, TimeSlot? right)
    {
        return EqualityComparer<TimeSlot>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two time slots are not equal.
    /// </summary>
    public static bool operator !=(TimeSlot? left, TimeSlot? right)
    {
        return !(left == right);
    }
} 