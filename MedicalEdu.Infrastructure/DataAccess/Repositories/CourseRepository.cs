using System.Linq.Expressions;
using MedicalEdu.Domain.DataAccess;
using MedicalEdu.Domain.DataAccess.Repositories;
using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalEdu.Infrastructure.DataAccess.Repositories;

public sealed class CourseRepository : ICourseRepository
{
    private readonly IMedicalEduDbContext _context;

    public CourseRepository(IMedicalEduDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets a course by its unique identifier.
    /// </summary>
    public Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return BaseQuery().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    /// <summary>
    /// Gets all courses with optional filtering based on publication status, activity status, and instructor ID.
    /// </summary>
    public Task<List<Course>> GetAllAsync(bool? isPublished = null, bool? isActive = null, Guid? instructorId = null, CancellationToken cancellationToken = default)
    {
        var query = BaseQuery();

        if (isPublished.HasValue)
            query = query.Where(c => c.IsPublished == isPublished.Value);

        if (instructorId.HasValue)
            query = query.Where(c => c.InstructorId == instructorId.Value);

        if (isActive.HasValue)
        {
            if (isActive.Value)
            {
                query = query.Where(c => c.DeletedAt == null);
            }
            else
            {
                query = query.Where(c => c.DeletedAt != null);
            }
        }

        return query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets courses with pagination and filtering.
    /// </summary>
    public async Task<(int TotalCount, T[] Results)> GetWithPagingAsync<T>(
        List<Expression<Func<Course, bool>>> conditions,
        Func<IQueryable<Course>, IQueryable<Course>> sortingFunc,
        (int Page, int PageSize) pagingOptions,
        Expression<Func<Course, T>> selector,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Course> query = _context.Courses.AsNoTracking();

        // Apply filter conditions
        foreach (var condition in conditions)
        {
            query = query.Where(condition);
        }

        // Apply sorting
        query = sortingFunc(query);

        // Get total count before pagination
        int total = await query.CountAsync(cancellationToken);

        // Apply pagination
        query = query.Skip(pagingOptions.Page * pagingOptions.PageSize).Take(pagingOptions.PageSize);

        // Apply selector and get results
        T[] results = await query.Select(selector).ToArrayAsync(cancellationToken);

        return (total, results);
    }

    /// <summary>
    /// Adds a new course to the repository.
    /// </summary>
    public Task AddAsync(Course course, CancellationToken cancellationToken = default) =>
        _context.Courses.AddAsync(course, cancellationToken).AsTask();

    /// <summary>
    /// Updates an existing course in the repository.
    /// </summary>
    public ValueTask UpdateAsync(Course course, CancellationToken cancellationToken = default)
    {
        _context.Courses.Update(course);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Removes a course from the repository.
    /// </summary>
    public ValueTask RemoveAsync(Course course, CancellationToken cancellationToken = default)
    {
        _context.Courses.Remove(course);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Saves all changes made in this repository to the database.
    /// </summary>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    /// <summary>
    /// Creates a base query with common includes for course operations.
    /// </summary>
    private IQueryable<Course> BaseQuery() =>
        _context.Courses.AsNoTracking()
            .Include(c => c.Instructor)
            .Include(c => c.AvailabilitySlots)
            .Include(c => c.CourseMaterials)
            .Include(c => c.CourseRatings)
            .Include(c => c.Enrollments);
}