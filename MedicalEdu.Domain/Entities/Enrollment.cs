using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class Enrollment
{
    /// <summary>
    /// Gets or sets the unique identifier for the enrollment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the student identifier.
    /// </summary>
    [Required]
    public Guid StudentId { get; set; }

    /// <summary>
    /// Gets or sets the course identifier.
    /// </summary>
    [Required]
    public Guid CourseId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the student enrolled.
    /// </summary>
    public DateTime EnrolledAt { get; set; }

    /// <summary>
    /// Gets or sets whether the enrollment is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the progress percentage (0-100).
    /// </summary>
    public int ProgressPercentage { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the course was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the enrollment was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual User Student { get; set; }
    public virtual Course Course { get; set; }
} 