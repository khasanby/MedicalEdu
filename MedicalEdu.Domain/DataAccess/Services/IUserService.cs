using MedicalEdu.Domain.Aggregates;

namespace MedicalEdu.Domain.DataAccess.Services;

/// <summary>
/// Service interface for user-related business operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user aggregate by their unique identifier.
    /// </summary>
    public Task<UserAggregate?> GetUserAggregateAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user with the specified details.
    /// </summary>
    public Task<UserAggregate> CreateUserAsync(string name, string email, string password, UserRole role, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirms a user's email address using the provided token.
    /// </summary>
    public Task<bool> ConfirmEmailAsync(Guid userId, string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes a user's password.
    /// </summary>
    public Task<bool> ChangePasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken = default);
}