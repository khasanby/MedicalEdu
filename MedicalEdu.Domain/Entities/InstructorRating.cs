using MedicalEdu.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class InstructorRating : IEntity<Guid>
{
    /// <summary>
    /// Gets or sets the unique identifier for the instructor rating.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the instructor identifier being rated.
    /// </summary>
    [Required]
    public Guid InstructorId { get; set; }

    /// <summary>
    /// Gets or sets the student identifier who gave the rating.
    /// </summary>
    [Required]
    public Guid StudentId { get; set; }

    /// <summary>
    /// Gets or sets the booking identifier this rating is for.
    /// </summary>
    [Required]
    public Guid BookingId { get; set; }

    /// <summary>
    /// Gets or sets the rating value (1-5 stars).
    /// </summary>
    [Range(1, 5)]
    public int Rating { get; set; }

    /// <summary>
    /// Gets or sets the review text.
    /// </summary>
    public string? Review { get; set; }

    /// <summary>
    /// Gets or sets whether this rating is public.
    /// </summary>
    public bool IsPublic { get; set; } = true;

    /// <summary>
    /// Gets or sets the date and time when the rating was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the rating was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Audit fields from IEntity
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    // Navigation properties
    public virtual User Instructor { get; set; }
    public virtual User Student { get; set; }
    public virtual Booking Booking { get; set; }
} 