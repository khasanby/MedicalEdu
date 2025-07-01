using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;

namespace MedicalEdu.Domain.Events;

public abstract class UserEvent : IDomainEvent
{
    public Guid Id { get; }
    public DateTime OccurredOn { get; }
    public Guid UserId { get; }

    protected UserEvent(Guid userId)
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        UserId = userId;
    }
}

public class UserCreatedEvent : UserEvent
{
    public string Email { get; }
    public UserRole Role { get; }

    public UserCreatedEvent(Guid userId, string email, UserRole role) : base(userId)
    {
        Email = email;
        Role = role;
    }
}

public class UserProfileUpdatedEvent : UserEvent
{
    public UserProfileUpdatedEvent(Guid userId) : base(userId)
    {
    }
}

public class UserProfilePictureUpdatedEvent : UserEvent
{
    public UserProfilePictureUpdatedEvent(Guid userId) : base(userId)
    {
    }
}

public class UserEmailConfirmedEvent : UserEvent
{
    public string Email { get; }

    public UserEmailConfirmedEvent(Guid userId, string email) : base(userId)
    {
        Email = email;
    }
}

public class UserEmailConfirmationTokenGeneratedEvent : UserEvent
{
    public string Email { get; }
    public string Token { get; }

    public UserEmailConfirmationTokenGeneratedEvent(Guid userId, string email, string token) : base(userId)
    {
        Email = email;
        Token = token;
    }
}

public class UserPasswordChangedEvent : UserEvent
{
    public UserPasswordChangedEvent(Guid userId) : base(userId)
    {
    }
}

public class UserDeactivatedEvent : UserEvent
{
    public UserDeactivatedEvent(Guid userId) : base(userId)
    {
    }
}

public class UserActivatedEvent : UserEvent
{
    public UserActivatedEvent(Guid userId) : base(userId)
    {
    }
} 