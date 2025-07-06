using MedicalEdu.Domain.DataAccess;
using MedicalEdu.Domain.DataAccess.Repositories;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MedicalEdu.Infrastructure.DataAccess.Repositories;

public sealed class BookingRepository : IBookingRepository
{
    private readonly IMedicalEduDbContext _context;

    public BookingRepository(IMedicalEduDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async ValueTask<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Include(b => b.Student)
            .Include(b => b.BookingPromoCodes)
            .Include(b => b.Payments)
            .Include(b => b.InstructorRatings)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<Booking>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Include(b => b.Student)
            .Include(b => b.BookingPromoCodes)
            .Include(b => b.Payments)
            .Include(b => b.InstructorRatings)
            .Where(b => b.StudentId == studentId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<Booking>> GetBySlotIdAsync(Guid slotId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .Include(b => b.Student)
            .Include(b => b.BookingPromoCodes)
            .Include(b => b.Payments)
            .Include(b => b.InstructorRatings)
            .Where(b => b.AvailabilitySlotId == slotId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<Booking>> GetAllAsync(BookingStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Bookings.AsNoTracking().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(b => b.Status == status.Value);
        }

        return await query
            .Include(b => b.Student)
            .Include(b => b.BookingPromoCodes)
            .Include(b => b.Payments)
            .Include(b => b.InstructorRatings)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        await _context.Bookings.AddAsync(booking, cancellationToken);
    }

    /// <inheritdoc/>
    public ValueTask UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        _context.Bookings.Update(booking);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public ValueTask RemoveAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        _context.Bookings.Remove(booking);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public async ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}