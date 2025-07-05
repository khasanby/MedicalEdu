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

    internal void UpdatePassword(Password newHash)
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
}