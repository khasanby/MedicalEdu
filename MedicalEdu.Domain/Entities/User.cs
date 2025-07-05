using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;
using TimeZone = MedicalEdu.Domain.ValueObjects.TimeZone;

namespace MedicalEdu.Domain.Entities;

public sealed class User : IEntity<Guid>
{
    /// <summary>
    /// Gets the unique identifier for the user.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// Gets the password hash for the user.
    /// </summary>
    public Password PasswordHash { get; private set; }

    /// <summary>
    /// Gets the role of the user.
    /// </summary>
    public UserRole Role { get; private set; }

    /// <summary>
    /// Gets the date and time when the user was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time when the user was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when the user was soft deleted.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; private set; }

    /// <summary>
    /// Gets if the user is currently active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the user's email is confirmed.
    /// </summary>
    public bool EmailConfirmed { get; private set; }

    /// <summary>
    /// Gets the email confirmation token.
    /// </summary>
    public string? EmailConfirmationToken { get; private set; }

    /// <summary>
    /// Gets the email confirmation token expiry date.
    /// </summary>
    public DateTimeOffset? EmailConfirmationTokenExpiry { get; private set; }

    /// <summary>
    /// Gets the password reset token.
    /// </summary>
    public string? PasswordResetToken { get; private set; }

    /// <summary>
    /// Gets the password reset token expiry date.
    /// </summary>
    public DateTimeOffset? PasswordResetTokenExpiry { get; private set; }

    /// <summary>
    /// Gets the user's time zone identifier.
    /// </summary>
    public TimeZone Zone { get; private set; }

    /// <summary>
    /// Gets the user's phone number.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Gets the user's profile picture URL.
    /// </summary>
    public string? ProfilePictureUrl { get; private set; }

    /// <summary>
    /// Gets the date and time when the user last logged in.
    /// </summary>
    public DateTimeOffset? LastLoginAt { get; private set; }

    /// <summary>
    /// Gets the number of failed login attempts.
    /// </summary>
    public int FailedLoginAttempts { get; private set; }

    /// <summary>
    /// Gets the date and time when the user account is locked until.
    /// </summary>
    public DateTimeOffset? LockedUntil { get; private set; }

    // Audit fields from IEntity
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }


    /// <summary>
    /// Initializes a new instance of the User class.
    /// </summary>
    /// <param name="id">The unique identifier for the user.</param>
    /// <param name="name">The name of the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="passwordHash">The password hash for the user.</param>
    /// <param name="role">The role of the user.</param>
    public User(Guid id, string name, Email email, Password passwordHash, UserRole role)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        Role = role;

        CreatedAt = DateTimeOffset.UtcNow;
        IsActive = true;
        Zone = TimeZone.Parse("UTC");

        EmailConfirmed = false;
        FailedLoginAttempts = 0;
    }
}