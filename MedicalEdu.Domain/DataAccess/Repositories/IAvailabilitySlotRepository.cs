using MedicalEdu.Domain.Entities;

namespace MedicalEdu.Domain.DataAccess.Repositories;

public interface IAvailabilitySlotRepository
{
    /// <summary>
    /// Gets an availability slot by its unique identifier.
    /// </summary>
    public ValueTask<AvailabilitySlot?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all availability slots with optional filtering.
    /// </summary>
    public Task<List<AvailabilitySlot>> GetAllAsync(
        Guid? instructorId = null,
        bool? isAvailable = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets availability slots for a specific instructor.
    /// </summary>
    public Task<List<AvailabilitySlot>> GetByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets available slots for a specific date range.
    /// </summary>
    public Task<List<AvailabilitySlot>> GetAvailableSlotsAsync(
        DateTime startDate,
        DateTime endDate,
        Guid? instructorId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new availability slot.
    /// </summary>
    public ValueTask AddAsync(AvailabilitySlot availabilitySlot, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing availability slot.
    /// </summary>
    public ValueTask UpdateAsync(AvailabilitySlot availabilitySlot, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an availability slot.
    /// </summary>
    public ValueTask DeleteAsync(AvailabilitySlot availabilitySlot, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all changes made to the repository.
    /// </summary>
    public ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}