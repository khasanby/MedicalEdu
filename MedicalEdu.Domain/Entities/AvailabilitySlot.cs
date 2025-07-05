using MedicalEdu.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class AvailabilitySlot : IEntity<Guid>
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
    /// Gets or sets the currency for the slot price.
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Gets or sets the maximum number of participants allowed in this slot.
    /// </summary>
    public int MaxParticipants { get; set; } = 1;

    /// <summary>
    /// Gets or sets the current number of participants in this slot.
    /// </summary>
    public int CurrentParticipants { get; set; } = 0;

    /// <summary>
    /// Gets or sets optional notes about this slot.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets whether this slot is part of a recurring pattern.
    /// </summary>
    public bool IsRecurring { get; set; }

    /// <summary>
    /// Gets or sets the recurring pattern as JSON for recurring slots.
    /// </summary>
    public string? RecurringPattern { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the slot was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the slot was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Audit fields from IEntity
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    // Navigation properties
    public virtual Course Course { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
} 