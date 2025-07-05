namespace MedicalEdu.Domain.Entities;

public sealed partial class CourseRating
{
    internal void UpdateRating(int newRating, string? modifiedBy = null)
    {
        if (newRating < 1 || newRating > 5)
            throw new ArgumentException("Rating must be between 1 and 5.", nameof(newRating));

        Rating = newRating;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateReview(string? newReview, string? modifiedBy = null)
    {
        Review = newReview;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void SetVisibility(bool isPublic, string? modifiedBy = null)
    {
        IsPublic = isPublic;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}