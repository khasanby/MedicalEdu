namespace MedicalEdu.Domain.Entities;

public sealed partial class UserSession
{
    internal void RefreshActivity(string? modifiedBy = null)
    {
        LastActivityAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal bool IsExpired()
    {
        return ExpiresAt <= DateTimeOffset.UtcNow;
    }

    internal void Extend(TimeSpan duration, string? modifiedBy = null)
    {
        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be positive.", nameof(duration));

        ExpiresAt = ExpiresAt.Add(duration);
        LastActivityAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}