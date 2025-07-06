using MedicalEdu.Domain.Aggregates;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;
using MedicalEdu.Domain.DataAccess.Repositories;
using MedicalEdu.Domain.DataAccess.Services;

namespace MedicalEdu.Application.Services;

/// <summary>
/// Service implementation for user-related business operations.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <inheritdoc/>
    public async Task<UserAggregate?> GetUserAggregateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        return user != null ? new UserAggregate(user) : null;
    }

    /// <inheritdoc/>
    public async Task<UserAggregate> CreateUserAsync(string name, string email, string password, UserRole role, CancellationToken cancellationToken = default)
    {
        // Create value objects
        var emailVo = Email.Create(email);
        var passwordHash = PasswordHash.Create(password);

        // Create user entity
        var user = new User(
            id: Guid.NewGuid(),
            name: name,
            email: emailVo,
            passwordHash: passwordHash,
            role: role);

        // Add to repository
        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        // Return aggregate
        return new UserAggregate(user);
    }

    /// <inheritdoc/>
    public async Task<bool> ConfirmEmailAsync(Guid userId, string token, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            return false;

        var aggregate = new UserAggregate(user);
        
        try
        {
            aggregate.ConfirmEmail(token);
            await _userRepository.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ChangePasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            return false;

        var aggregate = new UserAggregate(user);
        
        try
        {
            aggregate.ChangePassword(newPassword);
            await _userRepository.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
} 