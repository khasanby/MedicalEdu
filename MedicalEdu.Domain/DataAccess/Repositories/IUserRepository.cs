using MedicalEdu.Domain.Entities;

namespace MedicalEdu.Domain.DataAccess.Repositories;

/// <summary>
/// Repository interface for User entity operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by their unique identifier.
    /// </summary>
    public ValueTask<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    public ValueTask<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all users with optional filtering.
    /// </summary>
    public Task<List<User>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    public ValueTask AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user in the repository.
    /// </summary>
    public ValueTask UpdateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a user from the repository.
    /// </summary>
    public ValueTask RemoveAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all changes made in this repository to the database.
    /// </summary>
    public ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}