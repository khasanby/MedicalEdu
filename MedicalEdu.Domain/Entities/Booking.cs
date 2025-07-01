using MedicalEdu.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class Booking
{
    /// <summary>
    /// Gets or sets the unique identifier for the booking.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the course identifier for this booking.
    /// </summary>
    [Required]
    public Guid CourseId { get; set; }

    /// <summary>
    /// Gets or sets the student identifier who made the booking.
    /// </summary>
    [Required]
    public Guid StudentId { get; set; }

    /// <summary>
    /// Gets or sets the instructor identifier for this booking.
    /// </summary>
    [Required]
    public Guid InstructorId { get; set; }

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
    /// Gets or sets optional notes from the student.
    /// </summary>
    [MaxLength(1000)]
    public string? StudentNotes { get; set; }

    /// <summary>
    /// Gets or sets optional notes from the instructor.
    /// </summary>
    [MaxLength(1000)]
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
    [MaxLength(500)]
    public string? CancellationReason { get; set; }

    // Navigation properties
    public virtual Course Course { get; set; }
    public virtual User Student { get; set; }
    public virtual User Instructor { get; set; }
    public virtual AvailabilitySlot AvailabilitySlot { get; set; }
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
} 