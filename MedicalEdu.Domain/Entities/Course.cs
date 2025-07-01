using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class Course
{
    /// <summary>
    /// Gets or sets the unique identifier for the course.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the instructor of the course.
    /// </summary>
    [Required]
    public Guid InstructorId { get; set; }

    /// <summary>
    /// Gets or sets the title of the course.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the course.
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets whether the course is published and visible to students.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Gets or sets the course price for paid content.
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Gets or sets the course thumbnail image URL.
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the course was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the course was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual User Instructor { get; set; }
    public virtual ICollection<CourseMaterial> Materials { get; set; } = new List<CourseMaterial>();
    public virtual ICollection<AvailabilitySlot> AvailabilitySlots { get; set; } = new List<AvailabilitySlot>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}