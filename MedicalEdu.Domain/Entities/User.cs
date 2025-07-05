using System.ComponentModel.DataAnnotations;
using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;

namespace MedicalEdu.Domain.Entities;

public sealed class User : IEntity<Guid>
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
    /// Gets or sets the date and time when the user was soft deleted.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

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
    /// Gets or sets the password reset token.
    /// </summary>
    public string? PasswordResetToken { get; set; }

    /// <summary>
    /// Gets or sets the password reset token expiry date.
    /// </summary>
    public DateTime? PasswordResetTokenExpiry { get; set; }

    /// <summary>
    /// Gets or sets the user's time zone identifier.
    /// </summary>
    [MaxLength(50)]
    public string TimeZone { get; set; } = "UTC";

    /// <summary>
    /// Gets or sets the user's phone number.
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the user's profile picture URL.
    /// </summary>
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user last logged in.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Gets or sets the number of failed login attempts.
    /// </summary>
    public int FailedLoginAttempts { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user account is locked until.
    /// </summary>
    public DateTime? LockedUntil { get; set; }

    // Audit fields from IEntity
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();
    public virtual ICollection<Booking> BookingsAsStudent { get; set; } = new List<Booking>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<AvailabilitySlot> AvailabilitySlots { get; set; } = new List<AvailabilitySlot>();
    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
    public virtual ICollection<InstructorRating> InstructorRatingsGiven { get; set; } = new List<InstructorRating>();
    public virtual ICollection<InstructorRating> InstructorRatingsReceived { get; set; } = new List<InstructorRating>();
    public virtual ICollection<CourseRating> CourseRatings { get; set; } = new List<CourseRating>();
}