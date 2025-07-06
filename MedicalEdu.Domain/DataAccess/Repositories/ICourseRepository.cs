using MedicalEdu.Domain.Entities;

namespace MedicalEdu.Domain.DataAccess.Repositories;

public interface ICourseRepository
{
    /// <summary>
    /// Gets a course by its unique identifier.
    /// </summary>
    public Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all courses with optional filtering.
    /// </summary>
    public Task<List<Course>> GetAllAsync(bool? isPublished = null, bool? isActive = null, Guid? instructorId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets courses with pagination and filtering.
    /// </summary>
    /// <typeparam name="T">The type of data to return.</typeparam>
    /// <param name="conditions">List of filter conditions.</param>
    /// <param name="sortingFunc">Function to apply sorting.</param>
    /// <param name="pagingOptions">Pagination options (Page, PageSize).</param>
    /// <param name="selector">Function to select and project the data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Tuple containing total count and array of results.</returns>
    public Task<(int TotalCount, T[] Results)> GetWithPagingAsync<T>(
        List<System.Linq.Expressions.Expression<Func<Course, bool>>> conditions,
        Func<System.Linq.IQueryable<Course>, System.Linq.IQueryable<Course>> sortingFunc,
        (int Page, int PageSize) pagingOptions,
        System.Linq.Expressions.Expression<Func<Course, T>> selector,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new course to the repository.
    /// </summary>
    public Task AddAsync(Course course, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing course in the repository.
    /// </summary>
    public ValueTask UpdateAsync(Course course, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a course from the repository.
    /// </summary>
    public ValueTask RemoveAsync(Course course, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all changes made in this repository to the database.
    /// </summary>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}