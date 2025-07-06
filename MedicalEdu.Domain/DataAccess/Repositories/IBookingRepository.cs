using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;

namespace MedicalEdu.Domain.DataAccess.Repositories;

public interface IBookingRepository
{
    /// <summary>
    /// Gets a booking by its unique identifier.
    /// </summary>
    public ValueTask<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all bookings for a specific student.
    /// </summary>
    public Task<List<Booking>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all bookings for a specific availability slot.
    /// </summary>
    public Task<List<Booking>> GetBySlotIdAsync(Guid slotId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all bookings with optional filtering.
    /// </summary>
    public Task<List<Booking>> GetAllAsync(BookingStatus? status = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new booking to the repository.
    /// </summary>
    public ValueTask AddAsync(Booking booking, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing booking in the repository.
    /// </summary>
    public ValueTask UpdateAsync(Booking booking, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a booking from the repository.
    /// </summary>
    public ValueTask RemoveAsync(Booking booking, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all changes made in this repository to the database.
    /// </summary>
    public ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}