using MedicalEdu.Domain.Enums;

namespace MedicalEdu.Domain.Entities;

public sealed partial class Booking
{
    /// <summary>
    /// Confirm the booking: only pending bookings can be confirmed.
    /// </summary>
    internal void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new InvalidOperationException("Only pending bookings can be confirmed.");

        Status = BookingStatus.Confirmed;
        ConfirmedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Cancel the booking: only pending or confirmed can be cancelled.
    /// </summary>
    internal void Cancel(string reason)
    {
        if (Status is not (BookingStatus.Pending or BookingStatus.Confirmed))
            throw new InvalidOperationException("Only pending or confirmed bookings can be cancelled.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason is required.", nameof(reason));

        Status = BookingStatus.Cancelled;
        CancellationReason = reason;
        CancelledAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Mark the booking as completed: only confirmed bookings with past end time.
    /// </summary>
    internal void Complete()
    {
        if (Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed bookings can be completed.");

        Status = BookingStatus.Completed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Reschedule this booking, linking to the original.
    /// </summary>
    internal void Reschedule(Guid newSlotId)
    {
        if (Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed bookings can be rescheduled.");

        if (newSlotId == AvailabilitySlotId)
            throw new ArgumentException("New slot must differ from the current slot.", nameof(newSlotId));

        Status = BookingStatus.Rescheduled;
        RescheduledFromBookingId = Id;
        AvailabilitySlotId = newSlotId;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Update student or instructor notes.
    /// </summary>
    internal void SetStudentNotes(string notes)
    {
        StudentNotes = notes;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void SetInstructorNotes(string notes)
    {
        InstructorNotes = notes;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}