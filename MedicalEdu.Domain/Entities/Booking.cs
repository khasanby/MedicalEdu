using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class Booking : IEntity<Guid>
{
    /// <summary>
    /// Gets or sets the unique identifier for the booking.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the student identifier who made the booking.
    /// </summary>
    [Required]
    public Guid StudentId { get; set; }

    /// <summary>
    /// Gets or sets the availability slot identifier for this booking.
    /// </summary>
    [Required]
    public Guid AvailabilitySlotId { get; set; }

    /// <summary>
    /// Gets or sets the current status of the booking.
    /// </summary>
    public BookingStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the booking amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the currency for the booking amount.
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Gets or sets optional notes from the student.
    /// </summary>
    public string? StudentNotes { get; set; }

    /// <summary>
    /// Gets or sets optional notes from the instructor.
    /// </summary>
    public string? InstructorNotes { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the booking was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the booking was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the booking was confirmed.
    /// </summary>
    public DateTime? ConfirmedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the booking was cancelled.
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    /// <summary>
    /// Gets or sets the reason for cancellation.
    /// </summary>
    public string? CancellationReason { get; set; }

    /// <summary>
    /// Gets or sets the booking ID that this booking was rescheduled from.
    /// </summary>
    public Guid? RescheduledFromBookingId { get; set; }

    /// <summary>
    /// Gets or sets the meeting URL for virtual sessions.
    /// </summary>
    public string? MeetingUrl { get; set; }

    /// <summary>
    /// Gets or sets the meeting notes for post-session documentation.
    /// </summary>
    public string? MeetingNotes { get; set; }

    // Audit fields from IEntity
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    // Navigation properties
    public virtual User Student { get; set; }
    public virtual AvailabilitySlot AvailabilitySlot { get; set; }
    public virtual Booking? RescheduledFromBooking { get; set; }
    public virtual ICollection<Booking> RescheduledBookings { get; set; } = new List<Booking>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<BookingPromoCode> BookingPromoCodes { get; set; } = new List<BookingPromoCode>();
    public virtual InstructorRating? InstructorRating { get; set; }
} 