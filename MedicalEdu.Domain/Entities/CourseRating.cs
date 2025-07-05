using MedicalEdu.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class CourseRating : IEntity<Guid>
{
    /// <summary>
    /// Gets or sets the unique identifier for the course rating.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the course identifier being rated.
    /// </summary>
    [Required]
    public Guid CourseId { get; set; }

    /// <summary>
    /// Gets or sets the student identifier who gave the rating.
    /// </summary>
    [Required]
    public Guid StudentId { get; set; }

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
    public virtual Course Course { get; set; }
    public virtual User Student { get; set; }
} 