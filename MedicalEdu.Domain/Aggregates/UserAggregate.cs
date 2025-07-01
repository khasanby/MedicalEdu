using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Aggregates;

public sealed class UserAggregate : IAggregateRoot<Guid>
{


    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the email of the user.
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// Gets the password of the user.
    /// </summary>
    public Password Password { get; private set; }

    /// <summary>
    /// Gets the role of the user.
    /// </summary>
    public UserRole Role { get; private set; }

    /// <summary>
    /// Gets the date and time when the user was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the user is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the user's email is confirmed.
    /// </summary>
    public bool EmailConfirmed { get; private set; }

    /// <summary>
    /// Gets the email confirmation token.
    /// </summary>
    public string? EmailConfirmationToken { get; private set; }

    /// <summary>
    /// Gets the email confirmation token expiry date.
    /// </summary>
    public DateTime? EmailConfirmationTokenExpiry { get; private set; }

    /// <summary>
    /// Gets the time zone of the user.
    /// </summary>
    public string? TimeZone { get; private set; }

    /// <summary>
    /// Gets the phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; private set; }

    /// <summary>
    /// Gets the profile picture URL of the user.
    /// </summary>
    public string? ProfilePictureUrl { get; private set; }

    // Navigation properties
    public virtual ICollection<Course> CoursesAsInstructor { get; private set; } = new List<Course>();
    public virtual ICollection<Booking> BookingsAsStudent { get; private set; } = new List<Booking>();
    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();
    public virtual ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();
    public virtual ICollection<Notification> Notifications { get; private set; } = new List<Notification>();
    public virtual ICollection<AvailabilitySlot> AvailabilitySlots { get; private set; } = new List<AvailabilitySlot>();

    // Domain events
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private UserAggregate() { } // For EF Core

    public static UserAggregate Create(string name, Email email, Password password, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        var user = new UserAggregate
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Password = password,
            Role = role,
            IsActive = true,
            EmailConfirmed = false
        };

        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Email.Value, user.Role));
        return user;
    }

    public void UpdateProfile(string name, string? phoneNumber, string? timeZone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        PhoneNumber = phoneNumber;
        TimeZone = timeZone;
        UpdatedAt = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        AddDomainEvent(new UserProfileUpdatedEvent(Id));
    }

    public void UpdateProfilePicture(string profilePictureUrl)
    {
        ProfilePictureUrl = profilePictureUrl;
        UpdatedAt = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        AddDomainEvent(new UserProfilePictureUpdatedEvent(Id));
    }

    public void ConfirmEmail()
    {
        if (EmailConfirmed)
            throw new InvalidOperationException("Email is already confirmed");

        EmailConfirmed = true;
        EmailConfirmationToken = null;
        EmailConfirmationTokenExpiry = null;
        UpdatedAt = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        AddDomainEvent(new UserEmailConfirmedEvent(Id, Email.Value));
    }

    public void GenerateEmailConfirmationToken()
    {
        if (EmailConfirmed)
            throw new InvalidOperationException("Email is already confirmed");

        EmailConfirmationToken = Guid.NewGuid().ToString();
        EmailConfirmationTokenExpiry = DateTime.UtcNow.AddHours(24);
        UpdatedAt = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        AddDomainEvent(new UserEmailConfirmationTokenGeneratedEvent(Id, Email.Value, EmailConfirmationToken));
    }

    public bool ValidateEmailConfirmationToken(string token)
    {
        if (EmailConfirmed)
            return false;

        if (string.IsNullOrEmpty(EmailConfirmationToken) || EmailConfirmationToken != token)
            return false;

        if (EmailConfirmationTokenExpiry.HasValue && EmailConfirmationTokenExpiry.Value < DateTime.UtcNow)
            return false;

        return true;
    }

    public void ChangePassword(Password newPassword)
    {
        Password = newPassword;
        UpdatedAt = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        AddDomainEvent(new UserPasswordChangedEvent(Id));
    }

    public bool VerifyPassword(string plainTextPassword)
    {
        return Password.Verify(plainTextPassword);
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("User is already deactivated");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        AddDomainEvent(new UserDeactivatedEvent(Id));
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("User is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserActivatedEvent(Id));
    }

    public bool CanCreateCourse()
    {
        return IsActive && EmailConfirmed && Role == UserRole.Instructor;
    }

    public bool CanBookSession()
    {
        return IsActive && EmailConfirmed && Role == UserRole.Student;
    }

    public bool CanAccessCourse(Guid courseId)
    {
        // Admin can access all courses
        if (Role == UserRole.Admin)
            return true;

        // Instructor can access their own courses
        if (Role == UserRole.Instructor)
            return CoursesAsInstructor.Any(c => c.Id == courseId);

        // Student can access enrolled courses
        if (Role == UserRole.Student)
            return Enrollments.Any(e => e.CourseId == courseId && e.IsActive);

        return false;
    }

    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
} 