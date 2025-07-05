using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;
using TimeZone = MedicalEdu.Domain.ValueObjects.TimeZone;

namespace MedicalEdu.Domain.Aggregates;

public sealed class UserAggregate : IAggregateRoot<Guid>
{
    /// <summary>
    /// Holds domain events raised by this aggregate.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// The underlying User entity whose state is managed by this aggregate.
    /// </summary>
    private readonly User _user;

    /// <summary>
    /// Initializes a new instance of the UserAggregate with the given User entity.
    /// </summary>
    /// <param name="user">The User entity to wrap.</param>
    public UserAggregate(User user)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
    }

    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    public Guid Id => _user.Id;

    /// <summary>
    /// The user's display name.
    /// </summary>
    public string Name => _user.Name;

    /// <summary>
    /// The user's email address value object.
    /// </summary>
    public Email Email => _user.Email;

    /// <summary>
    /// The user's password hash value object.
    /// </summary>
    public Password PasswordHash => _user.PasswordHash;

    /// <summary>
    /// The user's role (admin, instructor, student, etc.).
    /// </summary>
    public UserRole Role => _user.Role;

    /// <summary>
    /// The timestamp when the user was created.
    /// </summary>
    public DateTimeOffset CreatedAt => _user.CreatedAt;

    /// <summary>
    /// The timestamp when the user was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt => _user.UpdatedAt;
    
    /// <summary>
    /// The timestamp when the user was soft-deleted, if any.
    /// </summary>
    public DateTimeOffset? DeletedAt => _user.DeletedAt;

    /// <summary>
    /// Whether the user is currently active.
    /// </summary>
    public bool IsActive => _user.IsActive;
    
    /// <summary>
    /// Whether the user's email has been confirmed.
    /// </summary>
    public bool EmailConfirmed => _user.EmailConfirmed;
    
    /// <summary>
    /// The current email confirmation token, if any.
    /// </summary>
    public string? EmailConfirmationToken => _user.EmailConfirmationToken;
    
    /// <summary>
    /// The expiry timestamp for the email confirmation token, if any.
    /// </summary>
    public DateTimeOffset? EmailConfirmationTokenExpiry => _user.EmailConfirmationTokenExpiry;
    
    /// <summary>
    /// The current password reset token, if any.
    /// </summary>
    public string? PasswordResetToken => _user.PasswordResetToken;
    
    /// <summary>
    /// The expiry timestamp for the password reset token, if any.
    /// </summary>
    public DateTimeOffset? PasswordResetTokenExpiry => _user.PasswordResetTokenExpiry;
    
    /// <summary>
    /// The user's time zone value object.
    /// </summary>
    public TimeZone Zone => _user.Zone;
   
    /// <summary>
    /// The user's phone number, if any.
    /// </summary>
    public string? PhoneNumber => _user.PhoneNumber;
    
    /// <summary>
    /// The user's profile picture URL, if any.
    /// </summary>
    public string? ProfilePictureUrl => _user.ProfilePictureUrl;
    
    /// <summary>
    /// The timestamp of the user's last login, if any.
    /// </summary>
    public DateTimeOffset? LastLoginAt => _user.LastLoginAt;
    
    /// <summary>
    /// The number of failed login attempts for the user.
    /// </summary>
    public int FailedLoginAttempts => _user.FailedLoginAttempts;
    
    /// <summary>
    /// The timestamp until which the user account is locked, if any.
    /// </summary>
    public DateTimeOffset? LockedUntil => _user.LockedUntil;
    
    /// <summary>
    /// The identifier of the user who created this user record, if tracked.
    /// </summary>
    public string? CreatedBy => _user.CreatedBy;
    
    /// <summary>
    /// The timestamp when the user was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified => _user.LastModified;
    
    /// <summary>
    /// The identifier of the user who last modified this user record, if tracked.
    /// </summary>
    public string? LastModifiedBy => _user.LastModifiedBy;

    #region Domain Events
    /// <summary>
    /// The collection of domain events raised by this aggregate.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    /// <summary>
    /// Clears all domain events from the aggregate.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion

    #region Domain Methods
    public void ConfirmEmail(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be empty.", nameof(token));
        if (_user.EmailConfirmed)
            throw new InvalidOperationException("Email is already confirmed.");
        if (_user.EmailConfirmationToken != token)
            throw new InvalidOperationException("Invalid confirmation token.");
        if (_user.EmailConfirmationTokenExpiry.HasValue && _user.EmailConfirmationTokenExpiry < DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Confirmation token has expired.");

        _user.MarkEmailConfirmed();
        //_domainEvents.Add(new UserEmailConfirmed(_user.Id));
    }

    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("Password cannot be empty.", nameof(newPassword));
        if (!_user.IsActive)
            throw new InvalidOperationException("Cannot change password for inactive user.");

        var hash = Password.Create(newPassword);
        _user.UpdatePassword(hash);
        //_domainEvents.Add(new UserPasswordChanged(_user.Id));
    }

    public void LockAccount(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Lock duration must be positive.", nameof(duration));
        if (_user.LockedUntil.HasValue && _user.LockedUntil > DateTimeOffset.UtcNow)
            throw new InvalidOperationException("User is already locked.");

        var until = DateTimeOffset.UtcNow.Add(duration);
        _user.SetLock(until);
        //_domainEvents.Add(new UserLocked(_user.Id, until));
    }

    #endregion
}