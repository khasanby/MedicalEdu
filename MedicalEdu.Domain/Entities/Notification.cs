using MedicalEdu.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class Notification
{
    /// <summary>
    /// Gets or sets the unique identifier for the notification.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the user identifier this notification is for.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the notification type.
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Gets or sets the notification title.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the notification message.
    /// </summary>
    [Required]
    [MaxLength(1000)]
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets whether the notification has been read.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Gets or sets whether an email was sent for this notification.
    /// </summary>
    public bool EmailSent { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the email was sent.
    /// </summary>
    public DateTime? EmailSentAt { get; set; }

    /// <summary>
    /// Gets or sets related entity identifier (booking, course, etc.).
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// Gets or sets the type of related entity.
    /// </summary>
    [MaxLength(50)]
    public string? RelatedEntityType { get; set; }

    /// <summary>
    /// Gets or sets additional metadata as JSON.
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the notification was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the notification was read.
    /// </summary>
    public DateTime? ReadAt { get; set; }

    // Navigation property
    public virtual User User { get; set; }
} 