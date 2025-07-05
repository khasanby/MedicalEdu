using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;
using TimeZone = MedicalEdu.Domain.ValueObjects.TimeZone;

namespace MedicalEdu.Domain.Entities;

public sealed partial class User : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the user.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the user's full name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets and sets privately the user's email address (value object).
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// Gets and sets privately the user's hashed password (value object).
    /// </summary>
    public Password PasswordHash { get; private set; }

    /// <summary>
    /// Gets and sets privately the user's role (student, instructor, admin, etc.).
    /// </summary>
    public UserRole Role { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the user was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the user was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the user was soft-deleted, if any.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately whether the user is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets and sets privately whether the user's email has been confirmed.
    /// </summary>
    public bool EmailConfirmed { get; private set; }

    /// <summary>
    /// Gets and sets privately the token used for email confirmation.
    /// </summary>
    public string? EmailConfirmationToken { get; private set; }

    /// <summary>
    /// Gets and sets privately the expiry date of the email confirmation token.
    /// </summary>
    public DateTimeOffset? EmailConfirmationTokenExpiry { get; private set; }

    /// <summary>
    /// Gets and sets privately the token used for password reset.
    /// </summary>
    public string? PasswordResetToken { get; private set; }

    /// <summary>
    /// Gets and sets privately the expiry date of the password reset token.
    /// </summary>
    public DateTimeOffset? PasswordResetTokenExpiry { get; private set; }

    /// <summary>
    /// Gets and sets privately the user's time zone (value object).
    /// </summary>
    public TimeZone Zone { get; private set; }

    /// <summary>
    /// Gets and sets privately the user's phone number.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Gets and sets privately the URL of the user's profile picture.
    /// </summary>
    public string? ProfilePictureUrl { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the user last logged in.
    /// </summary>
    public DateTimeOffset? LastLoginAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the number of failed login attempts.
    /// </summary>
    public int FailedLoginAttempts { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the user was locked until.
    /// </summary>
    public DateTimeOffset? LockedUntil { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the user was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the courses this user instructs.
    /// </summary>
    public IReadOnlyCollection<Course> InstructorCourses { get; private set; } 
        = new List<Course>();

    /// <summary>
    /// Gets and sets privately the bookings where this user is the student.
    /// </summary>
    public IReadOnlyCollection<Booking> StudentBookings { get; private set; } 
        = new List<Booking>();

    /// <summary>
    /// Gets and sets privately the enrollments where this user is the student.
    /// </summary>
    public IReadOnlyCollection<Enrollment> Enrollments { get; private set; } 
        = new List<Enrollment>();

    /// <summary>
    /// Gets and sets privately the course ratings provided by this user.
    /// </summary>
    public IReadOnlyCollection<CourseRating> CourseRatings { get; private set; } 
        = new List<CourseRating>();

    /// <summary>
    /// Gets and sets privately the payments made by this user.
    /// </summary>
    public IReadOnlyCollection<Payment> Payments { get; private set; } 
        = new List<Payment>();

    /// <summary>
    /// Gets and sets privately the notifications sent to this user.
    /// </summary>
    public IReadOnlyCollection<Notification> Notifications { get; private set; } 
        = new List<Notification>();

    /// <summary>
    /// Gets and sets privately the user sessions for this user.
    /// </summary>
    public IReadOnlyCollection<UserSession> UserSessions { get; private set; } 
        = new List<UserSession>();

    /// <summary>
    /// Gets and sets privately the instructor ratings given by this user.
    /// </summary>
    public IReadOnlyCollection<InstructorRating> InstructorRatingsGiven { get; private set; } 
        = new List<InstructorRating>();

    /// <summary>
    /// Gets and sets privately the instructor ratings received by this user.
    /// </summary>
    public IReadOnlyCollection<InstructorRating> InstructorRatingsReceived { get; private set; } 
        = new List<InstructorRating>();

    /// <summary>
    /// Gets and sets privately the availability slots created by this user as instructor.
    /// </summary>
    public IReadOnlyCollection<AvailabilitySlot> InstructorAvailabilitySlots { get; private set; } 
        = new List<AvailabilitySlot>();

    public User(Guid id, string name, Email email, Password passwordHash, UserRole role, string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("User ID is required.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        Id = id;
        Name = name;
        Role = role;
        CreatedAt = DateTimeOffset.UtcNow;
        IsActive = true;
        Zone = TimeZone.Parse("UTC");
        EmailConfirmed = false;
        FailedLoginAttempts = 0;
        CreatedBy = createdBy;
    }
}