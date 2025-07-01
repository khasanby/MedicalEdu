using MedicalEdu.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(254)]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the password hash for the user.
    /// </summary>
    [Required]
    public string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the role of the user.
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets if the user is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user's email is confirmed.
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Gets or sets the email confirmation token.
    /// </summary>
    public string? EmailConfirmationToken { get; set; }

    /// <summary>
    /// Gets or sets the email confirmation token expiry date.
    /// </summary>
    public DateTime? EmailConfirmationTokenExpiry { get; set; }

    /// <summary>
    /// Gets or sets the user's time zone identifier.
    /// </summary>
    [MaxLength(50)]
    public string? TimeZone { get; set; }

    /// <summary>
    /// Gets or sets the user's phone number.
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the user's profile picture URL.
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    // Navigation properties
    public virtual ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();
    public virtual ICollection<Booking> BookingsAsStudent { get; set; } = new List<Booking>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<AvailabilitySlot> AvailabilitySlots { get; set; } = new List<AvailabilitySlot>();
}