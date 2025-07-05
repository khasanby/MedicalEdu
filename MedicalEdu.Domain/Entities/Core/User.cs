using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;
using TimeZone = MedicalEdu.Domain.ValueObjects.TimeZone;

namespace MedicalEdu.Domain.Entities;

public sealed partial class User : IEntity<Guid>
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Password PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public bool IsActive { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public string? EmailConfirmationToken { get; private set; }
    public DateTimeOffset? EmailConfirmationTokenExpiry { get; private set; }
    public string? PasswordResetToken { get; private set; }
    public DateTimeOffset? PasswordResetTokenExpiry { get; private set; }
    public TimeZone Zone { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? ProfilePictureUrl { get; private set; }
    public DateTimeOffset? LastLoginAt { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public DateTimeOffset? LockedUntil { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? LastModified { get; private set; }
    public string? LastModifiedBy { get; private set; }

    public User(Guid id, string name, Email email, Password passwordHash, UserRole role)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        Role = role;
        CreatedAt = DateTimeOffset.UtcNow;
        IsActive = true;
        Zone = TimeZone.Parse("UTC");
        EmailConfirmed = false;
        FailedLoginAttempts = 0;
    }
}