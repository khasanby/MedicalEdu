using MedicalEdu.Domain.DataAccess;
using MedicalEdu.Domain.DataAccess.Repositories;
using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalEdu.Infrastructure.DataAccess.Repositories;

/// <summary>
/// Repository implementation for User entity operations.
/// </summary>
public sealed class UserRepository : IUserRepository
{
    private readonly IMedicalEduDbContext _context;

    public UserRepository(IMedicalEduDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async ValueTask<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.InstructorCourses)
            .Include(u => u.StudentBookings)
            .Include(u => u.Enrollments)
            .Include(u => u.CourseRatings)
            .Include(u => u.Payments)
            .Include(u => u.Notifications)
            .Include(u => u.UserSessions)
            .Include(u => u.InstructorRatingsGiven)
            .Include(u => u.InstructorRatingsReceived)
            .Include(u => u.InstructorAvailabilitySlots)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<User>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Users.AsNoTracking().AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(u => u.IsActive == isActive.Value);
        }

        return await query
            .Include(u => u.InstructorCourses)
            .Include(u => u.StudentBookings)
            .Include(u => u.Enrollments)
            .Include(u => u.CourseRatings)
            .Include(u => u.Payments)
            .Include(u => u.Notifications)
            .Include(u => u.UserSessions)
            .Include(u => u.InstructorRatingsGiven)
            .Include(u => u.InstructorRatingsReceived)
            .Include(u => u.InstructorAvailabilitySlots)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    /// <inheritdoc/>
    public ValueTask UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public ValueTask RemoveAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Remove(user);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public async ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}