using MedicalEdu.Domain.Abstractions;

namespace MedicalEdu.Domain.Entities;

public sealed partial class UserSession : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the user session.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the user this session belongs to.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets and sets privately the session token for authentication.
    /// </summary>
    public string SessionToken { get; private set; }

    /// <summary>
    /// Gets and sets privately the IP address of the client.
    /// </summary>
    public string? IpAddress { get; private set; }

    /// <summary>
    /// Gets and sets privately the user agent string of the client.
    /// </summary>
    public string? UserAgent { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the session expires.
    /// </summary>
    public DateTimeOffset ExpiresAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the session was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the session was last active.
    /// </summary>
    public DateTimeOffset LastActivityAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the session was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the user this session belongs to.
    /// </summary>
    public User User { get; private set; }

    public UserSession(
        Guid id,
        Guid userId,
        string sessionToken,
        string? ipAddress = null,
        string? userAgent = null,
        DateTimeOffset? expiresAt = null,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Session ID is required.", nameof(id));
        if (userId == Guid.Empty) throw new ArgumentException("User ID is required.", nameof(userId));
        if (string.IsNullOrWhiteSpace(sessionToken)) throw new ArgumentException("Session token is required.", nameof(sessionToken));

        Id = id;
        UserId = userId;
        SessionToken = sessionToken;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        ExpiresAt = expiresAt ?? DateTimeOffset.UtcNow.AddHours(1);
        CreatedAt = DateTimeOffset.UtcNow;
        LastActivityAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    private UserSession() { }
}