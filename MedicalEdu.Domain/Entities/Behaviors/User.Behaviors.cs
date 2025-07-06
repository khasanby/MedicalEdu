using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Entities;

public sealed partial class User
{
    internal void MarkEmailConfirmed()
    {
        if (EmailConfirmed) throw new InvalidOperationException("Already confirmed.");
        EmailConfirmed = true;
        EmailConfirmationToken = null;
        EmailConfirmationTokenExpiry = null;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void UpdatePassword(PasswordHash newHash)
    {
        if (!IsActive) throw new InvalidOperationException("Inactive users can't change password.");
        PasswordHash = newHash;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void SetLock(DateTimeOffset until)
    {
        if (LockedUntil.HasValue && LockedUntil > DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Already locked.");
        LockedUntil = until;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void SetEmailConfirmationToken(SessionToken token, DateTimeOffset expiry)
    {
        EmailConfirmationToken = token;
        EmailConfirmationTokenExpiry = expiry;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void SetPasswordResetToken(SessionToken token, DateTimeOffset expiry)
    {
        PasswordResetToken = token;
        PasswordResetTokenExpiry = expiry;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void VerifyAndConsumePasswordResetToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be empty.", nameof(token));
        
        if (PasswordResetToken != token)
            throw new InvalidOperationException("Invalid password reset token.");
        
        if (PasswordResetTokenExpiry.HasValue && PasswordResetTokenExpiry < DateTimeOffset.UtcNow)
            throw new InvalidOperationException("Password reset token has expired.");
        
        // Consume the token
        PasswordResetToken = null;
        PasswordResetTokenExpiry = null;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void ResetFailedLoginAttempts()
    {
        FailedLoginAttempts = 0;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void SetLastLogin(DateTimeOffset loginTime)
    {
        LastLoginAt = loginTime;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void IncrementFailedLoginAttempts()
    {
        FailedLoginAttempts++;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        Name = name;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void UpdatePhoneNumber(PhoneNumber? phoneNumber)
    {
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void UpdateProfilePicture(Url? profilePictureUrl)
    {
        ProfilePictureUrl = profilePictureUrl;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void UpdateTimeZone(TimeZoneId timeZone)
    {
        Zone = timeZone;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}