using Microsoft.EntityFrameworkCore;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.DataAccess.Repositories;
using MedicalEdu.Domain.DataAccess;

namespace MedicalEdu.Infrastructure.DataAccess.Repositories;

/// <summary>
/// Repository implementation for Course entity operations.
/// </summary>
public class CourseRepository : ICourseRepository
{
    private readonly IMedicalEduDbContext _context;

    public CourseRepository(IMedicalEduDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .AsNoTracking()
            .Include(c => c.Instructor)
            .Include(c => c.AvailabilitySlots)
            .Include(c => c.CourseMaterials)
            .Include(c => c.CourseRatings)
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<Course>> GetAllAsync(bool? isPublished = null, bool? isActive = null, Guid? instructorId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Courses.AsNoTracking().AsQueryable();

        if (isPublished.HasValue)
        {
            query = query.Where(c => c.IsPublished == isPublished.Value);
        }

        if (instructorId.HasValue)
        {
            query = query.Where(c => c.InstructorId == instructorId.Value);
        }

        // Always filter out soft-deleted by default
        var courses = await query
            .Include(c => c.Instructor)
            .Include(c => c.AvailabilitySlots)
            .Include(c => c.CourseMaterials)
            .Include(c => c.CourseRatings)
            .Include(c => c.Enrollments)
            .ToListAsync(cancellationToken);

        // If isActive is specified, filter in-memory using the computed property
        if (isActive.HasValue)
        {
            courses = courses.Where(c => c.IsActive == isActive.Value).ToList();
        }

        return courses;
    }

    /// <inheritdoc/>
    public async ValueTask AddAsync(Course course, CancellationToken cancellationToken = default)
    {
        await _context.Courses.AddAsync(course, cancellationToken);
    }

    /// <inheritdoc/>
    public ValueTask UpdateAsync(Course course, CancellationToken cancellationToken = default)
    {
        _context.Courses.Update(course);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public ValueTask RemoveAsync(Course course, CancellationToken cancellationToken = default)
    {
        _context.Courses.Remove(course);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public async ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}