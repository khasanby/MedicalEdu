namespace MedicalEdu.Domain.Entities;

public sealed partial class CourseProgress
{
    internal void MarkCompleted(string? modifiedBy = null)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Course material is already completed.");

        IsCompleted = true;
        CompletedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void AddTime(int seconds, string? modifiedBy = null)
    {
        if (seconds <= 0)
            throw new ArgumentException("Time spent must be positive.", nameof(seconds));

        TimeSpentSeconds += seconds;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void ResetProgress(string? modifiedBy = null)
    {
        IsCompleted = false;
        CompletedAt = null;
        TimeSpentSeconds = 0;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}