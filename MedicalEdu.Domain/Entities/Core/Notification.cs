using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;

namespace MedicalEdu.Domain.Entities;

public sealed partial class Notification : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the notification.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the user receiving the notification.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets and sets privately the type of notification.
    /// </summary>
    public NotificationType Type { get; private set; }

    /// <summary>
    /// Gets and sets privately the title of the notification.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets and sets privately the message content of the notification.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Gets and sets privately whether the notification has been read.
    /// </summary>
    public bool IsRead { get; private set; }

    /// <summary>
    /// Gets and sets privately whether an email was sent for this notification.
    /// </summary>
    public bool EmailSent { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the email was sent.
    /// </summary>
    public DateTimeOffset? EmailSentAt { get; private set; }

    /// <summary>
    /// Gets and sets privately whether an SMS was sent for this notification.
    /// </summary>
    public bool SmsSent { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the SMS was sent.
    /// </summary>
    public DateTimeOffset? SmsSentAt { get; private set; }

    /// <summary>
    /// Gets and sets privately whether a push notification was sent.
    /// </summary>
    public bool PushSent { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the push notification was sent.
    /// </summary>
    public DateTimeOffset? PushSentAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the related entity.
    /// </summary>
    public Guid? RelatedEntityId { get; private set; }

    /// <summary>
    /// Gets and sets privately the type of the related entity.
    /// </summary>
    public string? RelatedEntityType { get; private set; }

    /// <summary>
    /// Gets and sets privately additional metadata for the notification (JSON).
    /// </summary>
    public string? Metadata { get; private set; }

    /// <summary>
    /// Gets and sets privately the scheduled time for the notification.
    /// </summary>
    public DateTimeOffset? ScheduledFor { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the notification was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the notification was read.
    /// </summary>
    public DateTimeOffset? ReadAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the notification was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    public Notification(
        Guid id,
        Guid userId,
        NotificationType type,
        string title,
        string message,
        Guid? relatedEntityId = null,
        string? relatedEntityType = null,
        string? metadata = null,
        DateTimeOffset? scheduledFor = null,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Notification ID is required.", nameof(id));
        if (userId == Guid.Empty) throw new ArgumentException("User ID is required.", nameof(userId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("Message is required.", nameof(message));

        Id = id;
        UserId = userId;
        Type = type;
        Title = title;
        Message = message;
        IsRead = false;
        EmailSent = false;
        EmailSentAt = null;
        SmsSent = false;
        SmsSentAt = null;
        PushSent = false;
        PushSentAt = null;
        RelatedEntityId = relatedEntityId;
        RelatedEntityType = relatedEntityType;
        Metadata = metadata;
        ScheduledFor = scheduledFor;
        CreatedAt = DateTimeOffset.UtcNow;
        ReadAt = null;
        CreatedBy = createdBy;
    }

    private Notification() { }
} 