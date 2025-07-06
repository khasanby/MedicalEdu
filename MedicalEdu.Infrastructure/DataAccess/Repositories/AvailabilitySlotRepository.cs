using MedicalEdu.Domain.DataAccess;
using MedicalEdu.Domain.DataAccess.Repositories;
using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalEdu.Infrastructure.DataAccess.Repositories;

/// <summary>
/// Repository implementation for availability slot operations.
/// </summary>
public sealed class AvailabilitySlotRepository : IAvailabilitySlotRepository
{
    private readonly IMedicalEduDbContext _context;

    public AvailabilitySlotRepository(IMedicalEduDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async ValueTask<AvailabilitySlot?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AvailabilitySlots
            .AsNoTracking()
            .FirstOrDefaultAsync(slot => slot.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<AvailabilitySlot>> GetAllAsync(
        Guid? instructorId = null,
        bool? isAvailable = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AvailabilitySlots.AsNoTracking().AsQueryable();

        if (instructorId.HasValue)
        {
            query = query.Where(slot => slot.InstructorId == instructorId.Value);
        }

        if (isAvailable.HasValue)
        {
            if (isAvailable.Value)
            {
                query = query.Where(slot => slot.CurrentParticipants < slot.MaxParticipants && !slot.IsBooked);
            }
            else
            {
                query = query.Where(slot => !(slot.CurrentParticipants < slot.MaxParticipants && !slot.IsBooked));
            }
        }

        if (startDate.HasValue)
        {
            query = query.Where(slot => slot.StartTimeUtc >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(slot => slot.EndTimeUtc <= endDate.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<AvailabilitySlot>> GetByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken = default)
    {
        return await _context.AvailabilitySlots
            .AsNoTracking()
            .Where(slot => slot.InstructorId == instructorId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<List<AvailabilitySlot>> GetAvailableSlotsAsync(
        DateTime startDate,
        DateTime endDate,
        Guid? instructorId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.AvailabilitySlots
            .AsNoTracking()
            .Where(slot => slot.CurrentParticipants < slot.MaxParticipants && !slot.IsBooked &&
                          slot.StartTimeUtc >= startDate &&
                          slot.EndTimeUtc <= endDate);

        if (instructorId.HasValue)
        {
            query = query.Where(slot => slot.InstructorId == instructorId.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask AddAsync(AvailabilitySlot availabilitySlot, CancellationToken cancellationToken = default)
    {
        await _context.AvailabilitySlots.AddAsync(availabilitySlot, cancellationToken);
    }

    /// <inheritdoc/>
    public ValueTask UpdateAsync(AvailabilitySlot availabilitySlot, CancellationToken cancellationToken = default)
    {
        _context.AvailabilitySlots.Update(availabilitySlot);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public ValueTask DeleteAsync(AvailabilitySlot availabilitySlot, CancellationToken cancellationToken = default)
    {
        _context.AvailabilitySlots.Remove(availabilitySlot);
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public async ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}