namespace MedicalEdu.Domain.Entities;

public sealed partial class Enrollment
{
    internal void Complete(string? modifiedBy = null)
    {
        if (CompletedAt.HasValue)
            throw new InvalidOperationException("Enrollment is already completed.");

        CompletedAt = DateTimeOffset.UtcNow;
        ProgressPercentage = 100;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateProgress(int percent, string? modifiedBy = null)
    {
        if (percent < 0 || percent > 100)
            throw new ArgumentException("Progress percentage must be between 0 and 100.", nameof(percent));

        if (CompletedAt.HasValue)
            throw new InvalidOperationException("Cannot update progress on completed enrollment.");

        ProgressPercentage = percent;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void Deactivate(string? modifiedBy = null)
    {
        if (!IsActive)
            throw new InvalidOperationException("Enrollment is already inactive.");

        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void Reactivate(string? modifiedBy = null)
    {
        if (IsActive)
            throw new InvalidOperationException("Enrollment is already active.");

        IsActive = true;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void RecordAccess(string? modifiedBy = null)
    {
        LastAccessedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}
