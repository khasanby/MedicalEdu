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
    /// Adds a new course to the repository.
    /// </summary>
    public ValueTask AddAsync(Course course, CancellationToken cancellationToken = default);

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
    public ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}