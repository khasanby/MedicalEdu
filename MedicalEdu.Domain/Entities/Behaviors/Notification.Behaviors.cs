namespace MedicalEdu.Domain.Entities;

public sealed partial class Notification
{
    internal void MarkRead(string? modifiedBy = null)
    {
        if (IsRead)
            throw new InvalidOperationException("Notification is already read.");

        IsRead = true;
        ReadAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void MarkEmailSent(DateTimeOffset at, string? modifiedBy = null)
    {
        if (EmailSent)
            throw new InvalidOperationException("Email has already been sent for this notification.");

        EmailSent = true;
        EmailSentAt = at;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void MarkSmsSent(DateTimeOffset at, string? modifiedBy = null)
    {
        if (SmsSent)
            throw new InvalidOperationException("SMS has already been sent for this notification.");

        SmsSent = true;
        SmsSentAt = at;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void MarkPushSent(DateTimeOffset at, string? modifiedBy = null)
    {
        if (PushSent)
            throw new InvalidOperationException("Push notification has already been sent for this notification.");

        PushSent = true;
        PushSentAt = at;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}