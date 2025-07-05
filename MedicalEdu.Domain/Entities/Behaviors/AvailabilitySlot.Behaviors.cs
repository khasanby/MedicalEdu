namespace MedicalEdu.Domain.Entities;

public sealed partial class AvailabilitySlot
{
    internal void MarkBooked(string? modifiedBy = null)
    {
        if (IsBooked)
            throw new InvalidOperationException("Slot is already booked.");

        if (CurrentParticipants >= MaxParticipants)
            throw new InvalidOperationException("Slot is at maximum capacity.");

        IsBooked = true;
        CurrentParticipants++;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void ReleaseBooking(string? modifiedBy = null)
    {
        if (!IsBooked)
            throw new InvalidOperationException("Slot is not booked.");

        if (CurrentParticipants <= 0)
            throw new InvalidOperationException("No participants to remove.");

        IsBooked = false;
        CurrentParticipants--;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void AddParticipant(string? modifiedBy = null)
    {
        if (CurrentParticipants >= MaxParticipants)
            throw new InvalidOperationException("Slot is at maximum capacity.");

        CurrentParticipants++;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void RemoveParticipant(string? modifiedBy = null)
    {
        if (CurrentParticipants <= 0)
            throw new InvalidOperationException("No participants to remove.");

        CurrentParticipants--;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdatePrice(decimal newPrice, string? modifiedBy = null)
    {
        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(newPrice));

        Price = newPrice;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void SetRecurring(string pattern, string? modifiedBy = null)
    {
        if (string.IsNullOrWhiteSpace(pattern))
            throw new ArgumentException("Recurring pattern cannot be empty.", nameof(pattern));

        IsRecurring = true;
        RecurringPattern = pattern;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void CancelRecurring(string? modifiedBy = null)
    {
        IsRecurring = false;
        RecurringPattern = null;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateNotes(string? notes, string? modifiedBy = null)
    {
        Notes = notes;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateTime(DateTimeOffset newStartTime, DateTimeOffset newEndTime, string? modifiedBy = null)
    {
        if (newEndTime <= newStartTime)
            throw new ArgumentException("End time must be after start time.", nameof(newEndTime));

        StartTimeUtc = newStartTime;
        EndTimeUtc = newEndTime;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}