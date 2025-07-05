using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Entities;

public sealed partial class Booking : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the booking.
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the unique identifier of the student making the booking.
    /// </summary>
    public Guid StudentId { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the student user who made this booking.
    /// </summary>
    public User Student { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the unique identifier of the reserved availability slot.
    /// </summary>
    public Guid AvailabilitySlotId { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the current status of the booking (pending, confirmed, cancelled, etc.).
    /// </summary>
    public BookingStatus Status { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the total amount for the booking.
    /// </summary>
    public decimal Amount { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the ISO currency code for the booking amount (value object).
    /// </summary>
    public Currency Currency { get; private set; }
    
    /// <summary>
    /// Gets and sets privately optional notes provided by the student.
    /// </summary>
    public string? StudentNotes { get; private set; }
    
    /// <summary>
    /// Gets and sets privately optional notes provided by the instructor.
    /// </summary>
    public string? InstructorNotes { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the timestamp when the booking was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the timestamp when the booking was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the timestamp when the booking was confirmed, if applicable.
    /// </summary>
    public DateTimeOffset? ConfirmedAt { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the timestamp when the booking was cancelled, if applicable.
    /// </summary>
    public DateTimeOffset? CancelledAt { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the reason for cancellation, if the booking was cancelled.
    /// </summary>
    public string? CancellationReason { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the ID of the original booking if this booking is a reschedule.
    /// </summary>
    public Guid? RescheduledFromBookingId { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the meeting URL for virtual sessions, if applicable.
    /// </summary>
    public string? MeetingUrl { get; private set; }
    
    /// <summary>
    /// Gets and sets privately notes about the meeting, if applicable.
    /// </summary>
    public string? MeetingNotes { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the identifier of the user who created this booking record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the timestamp when the booking was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }
    
    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this booking record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the promo codes applied to this booking.
    /// </summary>
    public IReadOnlyCollection<BookingPromoCode> BookingPromoCodes { get; private set; } 
        = new List<BookingPromoCode>();

    /// <summary>
    /// Gets and sets privately the payments for this booking.
    /// </summary>
    public IReadOnlyCollection<Payment> Payments { get; private set; } 
        = new List<Payment>();

    /// <summary>
    /// Gets and sets privately the instructor ratings for this booking.
    /// </summary>
    public IReadOnlyCollection<InstructorRating> InstructorRatings { get; private set; } 
        = new List<InstructorRating>();

    public Booking(
        Guid id,
        Guid studentId,
        Guid availabilitySlotId,
        decimal amount,
        Currency currency,
        string? studentNotes = null,
        string? instructorNotes = null,
        string? meetingUrl = null,
        string? meetingNotes = null,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Booking ID is required.", nameof(id));
        if (studentId == Guid.Empty) throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (availabilitySlotId == Guid.Empty) throw new ArgumentException("Slot ID is required.", nameof(availabilitySlotId));
        if (amount < 0) throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));

        Id = id;
        StudentId = studentId;
        AvailabilitySlotId = availabilitySlotId;
        Status = BookingStatus.Pending;
        Amount = amount;
        StudentNotes = studentNotes;
        InstructorNotes = instructorNotes;
        MeetingUrl = meetingUrl;
        MeetingNotes = meetingNotes;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }
}