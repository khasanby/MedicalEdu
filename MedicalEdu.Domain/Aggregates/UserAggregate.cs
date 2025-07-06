using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;
using MedicalEdu.Domain.Events;
using TimeZone = MedicalEdu.Domain.ValueObjects.TimeZoneId;

namespace MedicalEdu.Domain.Aggregates;

/// <summary>
/// Represents the user aggregate root that encapsulates user identity and security operations.
/// </summary>
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
    /// <param name="user">The user entity to encapsulate.</param>
    public UserAggregate(User user)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
    }

    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public Guid Id => _user.Id;

    /// <summary>
    /// Gets the user's display name.
    /// </summary>
    public string Name => _user.Name;

    /// <summary>
    /// Gets the user's email address value object.
    /// </summary>
    public Email Email => _user.Email;

    /// <summary>
    /// Gets the user's password hash value object.
    /// </summary>
    public PasswordHash PasswordHash => _user.PasswordHash;

    /// <summary>
    /// Gets the user's role (admin, instructor, student, etc.).
    /// </summary>
    public UserRole Role => _user.Role;

    /// <summary>
    /// Gets the timestamp when the user was created.
    /// </summary>
    public DateTimeOffset CreatedAt => _user.CreatedAt;

    /// <summary>
    /// Gets the timestamp when the user was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt => _user.UpdatedAt;

    /// <summary>
    /// Gets the timestamp when the user was soft-deleted, if any.
    /// </summary>
    public DateTimeOffset? DeletedAt => _user.DeletedAt;

    /// <summary>
    /// Gets whether the user is currently active.
    /// </summary>
    public bool IsActive => _user.IsActive;

    /// <summary>
    /// Gets whether the user's email has been confirmed.
    /// </summary>
    public bool EmailConfirmed => _user.EmailConfirmed;

    /// <summary>
    /// Gets the current email confirmation token, if any.
    /// </summary>
    public string? EmailConfirmationToken => _user.EmailConfirmationToken;

    /// <summary>
    /// Gets the expiry timestamp for the email confirmation token, if any.
    /// </summary>
    public DateTimeOffset? EmailConfirmationTokenExpiry => _user.EmailConfirmationTokenExpiry;

    /// <summary>
    /// Gets the current password reset token, if any.
    /// </summary>
    public string? PasswordResetToken => _user.PasswordResetToken;

    /// <summary>
    /// Gets the expiry timestamp for the password reset token, if any.
    /// </summary>
    public DateTimeOffset? PasswordResetTokenExpiry => _user.PasswordResetTokenExpiry;

    /// <summary>
    /// Gets the user's time zone value object.
    /// </summary>
    public TimeZone Zone => _user.Zone;

    /// <summary>
    /// Gets the user's phone number, if any.
    /// </summary>
    public PhoneNumber? PhoneNumber => _user.PhoneNumber;

    /// <summary>
    /// Gets the user's profile picture URL, if any.
    /// </summary>
    public Url? ProfilePictureUrl => _user.ProfilePictureUrl;

    /// <summary>
    /// Gets the timestamp of the user's last login, if any.
    /// </summary>
    public DateTimeOffset? LastLoginAt => _user.LastLoginAt;

    /// <summary>
    /// Gets the number of failed login attempts for the user.
    /// </summary>
    public int FailedLoginAttempts => _user.FailedLoginAttempts;

    /// <summary>
    /// Gets the timestamp until which the user account is locked, if any.
    /// </summary>
    public DateTimeOffset? LockedUntil => _user.LockedUntil;

    /// <summary>
    /// Gets the identifier of the user who created this user record, if tracked.
    /// </summary>
    public string? CreatedBy => _user.CreatedBy;

    /// <summary>
    /// Gets the timestamp when the user was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified => _user.LastModified;

    /// <summary>
    /// Gets the identifier of the user who last modified this user record, if tracked.
    /// </summary>
    public string? LastModifiedBy => _user.LastModifiedBy;

    #region Domain Events
    /// <summary>
    /// Gets the collection of domain events raised by this aggregate.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clears all domain events from the aggregate.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
    #endregion

    #region Domain Methods
    /// <summary>
    /// Confirms the user's email address using the provided token.
    /// </summary>
    /// <param name="token">The email confirmation token.</param>
    /// <exception cref="ArgumentException">Thrown when token is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when email is already confirmed or token is invalid/expired.</exception>
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
        _domainEvents.Add(new UserEmailConfirmed(_user.Id));
    }

    /// <summary>
    /// Changes the user's password to the new password.
    /// </summary>
    /// <param name="newPassword">The new password to set.</param>
    /// <exception cref="ArgumentException">Thrown when password is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when user is inactive.</exception>
    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            throw new ArgumentException("Password cannot be empty.", nameof(newPassword));
        if (!_user.IsActive)
            throw new InvalidOperationException("Cannot change password for inactive user.");

        var hash = PasswordHash.Create(newPassword);
        _user.UpdatePassword(hash);
        _domainEvents.Add(new UserPasswordChanged(_user.Id));
    }

    /// <summary>
    /// Locks the user account for the specified duration.
    /// </summary>
    /// <param name="duration">The duration to lock the account for.</param>
    /// <exception cref="ArgumentException">Thrown when duration is not positive.</exception>
    /// <exception cref="InvalidOperationException">Thrown when user is already locked.</exception>
    public void LockAccount(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Lock duration must be positive.", nameof(duration));
        if (_user.LockedUntil.HasValue && _user.LockedUntil > DateTimeOffset.UtcNow)
            throw new InvalidOperationException("User is already locked.");

        var until = DateTimeOffset.UtcNow.Add(duration);
        _user.SetLock(until);
        _domainEvents.Add(new UserLocked(_user.Id, until));
    }

    /// <summary>
    /// Generates a new email confirmation token valid for the specified duration.
    /// </summary>
    /// <param name="validFor">The duration the token should be valid for.</param>
    public void GenerateEmailConfirmationToken(TimeSpan validFor)
    {
        var token = SessionToken.Create();
        _user.SetEmailConfirmationToken(token, DateTimeOffset.UtcNow.Add(validFor));
        _domainEvents.Add(new UserEmailConfirmationTokenRequested(_user.Id, token));
    }

    /// <summary>
    /// Generates a new password reset token valid for the specified duration.
    /// </summary>
    /// <param name="validFor">The duration the token should be valid for.</param>
    public void GeneratePasswordResetToken(TimeSpan validFor)
    {
        var token = SessionToken.Create();
        _user.SetPasswordResetToken(token, DateTimeOffset.UtcNow.Add(validFor));
        _domainEvents.Add(new UserPasswordResetRequested(_user.Id, token));
    }

    /// <summary>
    /// Resets the user's password using the provided token.
    /// </summary>
    /// <param name="token">The password reset token.</param>
    /// <param name="newPassword">The new password to set.</param>
    /// <exception cref="ArgumentException">Thrown when token or password is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when token is invalid or expired.</exception>
    public void ResetPassword(string token, string newPassword)
    {
        _user.VerifyAndConsumePasswordResetToken(token);
        var hash = PasswordHash.Create(newPassword);
        _user.UpdatePassword(hash);
        _domainEvents.Add(new UserPasswordReset(_user.Id));
    }

    /// <summary>
    /// Records a successful login attempt for the user.
    /// </summary>
    public void RecordLoginSuccess()
    {
        _user.ResetFailedLoginAttempts();
        _user.SetLastLogin(DateTimeOffset.UtcNow);
        _domainEvents.Add(new UserLoggedIn(_user.Id));
    }

    /// <summary>
    /// Records a failed login attempt and potentially locks the account.
    /// </summary>
    /// <param name="maxAllowed">The maximum allowed failed attempts before locking.</param>
    /// <param name="lockDuration">The duration to lock the account for if max attempts exceeded.</param>
    public void RecordLoginFailure(int maxAllowed, TimeSpan lockDuration)
    {
        _user.IncrementFailedLoginAttempts();
        _domainEvents.Add(new UserLoginFailed(_user.Id));
        if (_user.FailedLoginAttempts >= maxAllowed)
            _domainEvents.Add(new UserLocked(_user.Id, DateTimeOffset.UtcNow.Add(lockDuration)));
    }

    /// <summary>
    /// Updates the user's display name.
    /// </summary>
    /// <param name="name">The new display name.</param>
    public void UpdateName(string name)
    {
        _user.UpdateName(name);
        _domainEvents.Add(new UserProfileUpdated(_user.Id));
    }

    /// <summary>
    /// Updates the user's phone number.
    /// </summary>
    /// <param name="phoneNumber">The new phone number, or null to remove.</param>
    public void UpdatePhoneNumber(PhoneNumber? phoneNumber)
    {
        _user.UpdatePhoneNumber(phoneNumber);
        _domainEvents.Add(new UserProfileUpdated(_user.Id));
    }

    /// <summary>
    /// Updates the user's profile picture URL.
    /// </summary>
    /// <param name="profilePictureUrl">The new profile picture URL, or null to remove.</param>
    public void UpdateProfilePicture(Url? profilePictureUrl)
    {
        _user.UpdateProfilePicture(profilePictureUrl);
        _domainEvents.Add(new UserProfileUpdated(_user.Id));
    }

    /// <summary>
    /// Updates the user's time zone.
    /// </summary>
    /// <param name="timeZone">The new time zone.</param>
    public void UpdateTimeZone(TimeZoneId timeZone)
    {
        _user.UpdateTimeZone(timeZone);
        _domainEvents.Add(new UserProfileUpdated(_user.Id));
    }

    /// <summary>
    /// Activates the user account.
    /// </summary>
    public void Activate()
    {
        _user.Activate();
        _domainEvents.Add(new UserActivated(_user.Id));
    }

    /// <summary>
    /// Deactivates the user account.
    /// </summary>
    public void Deactivate()
    {
        _user.Deactivate();
        _domainEvents.Add(new UserDeactivated(_user.Id));
    }
    #endregion
}