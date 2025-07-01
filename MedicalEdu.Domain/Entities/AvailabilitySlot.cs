using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class AvailabilitySlot
{
    /// <summary>
    /// Gets or sets the unique identifier for the availability slot.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the course identifier this slot is for.
    /// </summary>
    [Required]
    public Guid CourseId { get; set; }

    /// <summary>
    /// Gets or sets the instructor identifier who owns this slot.
    /// </summary>
    [Required]
    public Guid InstructorId { get; set; }

    /// <summary>
    /// Gets or sets the start date and time of the slot in UTC.
    /// </summary>
    public DateTime StartTimeUtc { get; set; }

    /// <summary>
    /// Gets or sets the end date and time of the slot in UTC.
    /// </summary>
    public DateTime EndTimeUtc { get; set; }

    /// <summary>
    /// Gets or sets whether this slot is already booked.
    /// </summary>
    public bool IsBooked { get; set; }

    /// <summary>
    /// Gets or sets the price for booking this slot.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets optional notes about this slot.
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the slot was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the slot was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Course Course { get; set; }
    public virtual User Instructor { get; set; }
    public virtual Booking? Booking { get; set; }
} 