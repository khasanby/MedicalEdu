using MedicalEdu.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class Course : IEntity<Guid>
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
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the short description of the course.
    /// </summary>
    [MaxLength(500)]
    public string? ShortDescription { get; set; }

    /// <summary>
    /// Gets or sets whether the course is published and visible to students.
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Gets or sets the course price for paid content.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the currency for the course price.
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Gets or sets the course thumbnail image URL.
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Gets or sets the video intro URL for the course.
    /// </summary>
    public string? VideoIntroUrl { get; set; }

    /// <summary>
    /// Gets or sets the duration of the course in minutes.
    /// </summary>
    public int? DurationMinutes { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of students allowed in the course.
    /// </summary>
    public int? MaxStudents { get; set; }

    /// <summary>
    /// Gets or sets the category of the course.
    /// </summary>
    [MaxLength(100)]
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the tags for the course as JSON array.
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the course was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the course was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the course was published.
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the course was soft deleted.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the course was last modified.
    /// </summary>
    public DateTime? LastModified { get; set; }

    /// <summary>
    /// Gets or sets the user who last modified the course.
    /// </summary>
    public string? LastModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the user who created the course.
    /// </summary>
    public string? CreatedBy { get; set; }

    // Navigation properties
    public virtual User Instructor { get; set; }
    public virtual ICollection<CourseMaterial> Materials { get; set; } = new List<CourseMaterial>();
    public virtual ICollection<AvailabilitySlot> AvailabilitySlots { get; set; } = new List<AvailabilitySlot>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<CourseRating> CourseRatings { get; set; } = new List<CourseRating>();
}