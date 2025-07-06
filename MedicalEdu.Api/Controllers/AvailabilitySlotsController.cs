using Microsoft.AspNetCore.Mvc;
using MedicalEdu.Domain.Aggregates;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Api.Controllers;

/// <summary>
/// Controller for availability slot-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AvailabilitySlotsController : ControllerBase
{
    private readonly IAvailabilitySlotRepository _availabilitySlotRepository;
    private readonly IUserRepository _userRepository;

    public AvailabilitySlotsController(
        IAvailabilitySlotRepository availabilitySlotRepository,
        IUserRepository userRepository)
    {
        _availabilitySlotRepository = availabilitySlotRepository ?? throw new ArgumentNullException(nameof(availabilitySlotRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Gets an availability slot by its unique identifier.
    /// </summary>
    /// <param name="id">The slot's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The slot if found; otherwise, 404 Not Found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<AvailabilitySlot>> GetAvailabilitySlot(Guid id, CancellationToken cancellationToken)
    {
        var slot = await _availabilitySlotRepository.GetByIdAsync(id, cancellationToken);
        
        if (slot == null)
        {
            return NotFound();
        }

        return Ok(slot);
    }

    /// <summary>
    /// Gets all availability slots with optional filtering.
    /// </summary>
    /// <param name="instructorId">Optional filter by instructor.</param>
    /// <param name="isAvailable">Optional filter for available slots only.</param>
    /// <param name="startDate">Optional filter by start date.</param>
    /// <param name="endDate">Optional filter by end date.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of availability slots.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AvailabilitySlot>>> GetAvailabilitySlots(
        [FromQuery] Guid? instructorId = null,
        [FromQuery] bool? isAvailable = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var slots = await _availabilitySlotRepository.GetAllAsync(instructorId, isAvailable, startDate, endDate, cancellationToken);
        return Ok(slots);
    }

    /// <summary>
    /// Creates a new availability slot.
    /// </summary>
    /// <param name="request">The slot creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created slot.</returns>
    [HttpPost]
    public async Task<ActionResult<AvailabilitySlot>> CreateAvailabilitySlot([FromBody] CreateAvailabilitySlotRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate instructor exists
            var instructor = await _userRepository.GetByIdAsync(request.InstructorId, cancellationToken);
            if (instructor == null)
            {
                return BadRequest("Instructor not found.");
            }

            // Create value objects
            var timeZone = TimeZone.Create(request.TimeZone);
            var recurringSchedule = request.IsRecurring && request.RecurringSchedule != null 
                ? RecurringSchedule.Create(
                    request.RecurringSchedule.DayOfWeek,
                    request.RecurringSchedule.StartTime,
                    request.RecurringSchedule.EndTime,
                    request.RecurringSchedule.Frequency,
                    request.RecurringSchedule.EndDate)
                : null;

            // Create availability slot entity
            var slot = new AvailabilitySlot(
                id: Guid.NewGuid(),
                instructorId: request.InstructorId,
                startTime: request.StartTime,
                endTime: request.EndTime,
                timeZone: timeZone,
                maxCapacity: request.MaxCapacity,
                isRecurring: request.IsRecurring,
                recurringSchedule: recurringSchedule,
                notes: request.Notes);

            // Add to repository
            await _availabilitySlotRepository.AddAsync(slot, cancellationToken);
            await _availabilitySlotRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetAvailabilitySlot), new { id = slot.Id }, slot);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing availability slot.
    /// </summary>
    /// <param name="id">The slot's unique identifier.</param>
    /// <param name="request">The slot update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAvailabilitySlot(Guid id, [FromBody] UpdateAvailabilitySlotRequest request, CancellationToken cancellationToken)
    {
        var slot = await _availabilitySlotRepository.GetByIdAsync(id, cancellationToken);
        
        if (slot == null)
        {
            return NotFound();
        }

        try
        {
            // Update slot properties
            slot.UpdateStartTime(request.StartTime);
            slot.UpdateEndTime(request.EndTime);
            slot.UpdateMaxCapacity(request.MaxCapacity);
            slot.UpdateNotes(request.Notes);

            if (request.TimeZone != null)
            {
                slot.UpdateTimeZone(TimeZone.Create(request.TimeZone));
            }

            await _availabilitySlotRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Activates an availability slot.
    /// </summary>
    /// <param name="id">The slot's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPost("{id}/activate")]
    public async Task<ActionResult> ActivateAvailabilitySlot(Guid id, CancellationToken cancellationToken)
    {
        var slot = await _availabilitySlotRepository.GetByIdAsync(id, cancellationToken);
        
        if (slot == null)
        {
            return NotFound();
        }

        try
        {
            slot.Activate();
            await _availabilitySlotRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deactivates an availability slot.
    /// </summary>
    /// <param name="id">The slot's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPost("{id}/deactivate")]
    public async Task<ActionResult> DeactivateAvailabilitySlot(Guid id, CancellationToken cancellationToken)
    {
        var slot = await _availabilitySlotRepository.GetByIdAsync(id, cancellationToken);
        
        if (slot == null)
        {
            return NotFound();
        }

        try
        {
            slot.Deactivate();
            await _availabilitySlotRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Books a slot (reduces capacity).
    /// </summary>
    /// <param name="id">The slot's unique identifier.</param>
    /// <param name="request">The booking request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPost("{id}/book")]
    public async Task<ActionResult> BookSlot(Guid id, [FromBody] BookSlotRequest request, CancellationToken cancellationToken)
    {
        var slot = await _availabilitySlotRepository.GetByIdAsync(id, cancellationToken);
        
        if (slot == null)
        {
            return NotFound();
        }

        try
        {
            slot.BookSlot(request.Quantity);
            await _availabilitySlotRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Cancels a slot booking (increases capacity).
    /// </summary>
    /// <param name="id">The slot's unique identifier.</param>
    /// <param name="request">The cancellation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPost("{id}/cancel-booking")]
    public async Task<ActionResult> CancelSlotBooking(Guid id, [FromBody] CancelSlotBookingRequest request, CancellationToken cancellationToken)
    {
        var slot = await _availabilitySlotRepository.GetByIdAsync(id, cancellationToken);
        
        if (slot == null)
        {
            return NotFound();
        }

        try
        {
            slot.CancelBooking(request.Quantity);
            await _availabilitySlotRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gets availability slots for a specific instructor.
    /// </summary>
    /// <param name="instructorId">The instructor's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of instructor's availability slots.</returns>
    [HttpGet("instructor/{instructorId}")]
    public async Task<ActionResult<IEnumerable<AvailabilitySlot>>> GetInstructorAvailabilitySlots(Guid instructorId, CancellationToken cancellationToken)
    {
        var slots = await _availabilitySlotRepository.GetByInstructorIdAsync(instructorId, cancellationToken);
        return Ok(slots);
    }

    /// <summary>
    /// Gets available slots for a specific date range.
    /// </summary>
    /// <param name="startDate">Start date for the range.</param>
    /// <param name="endDate">End date for the range.</param>
    /// <param name="instructorId">Optional filter by instructor.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of available slots.</returns>
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<AvailabilitySlot>>> GetAvailableSlots(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] Guid? instructorId = null,
        CancellationToken cancellationToken = default)
    {
        var slots = await _availabilitySlotRepository.GetAvailableSlotsAsync(startDate, endDate, instructorId, cancellationToken);
        return Ok(slots);
    }
}

/// <summary>
/// Request model for creating a new availability slot.
/// </summary>
public class CreateAvailabilitySlotRequest
{
    public Guid InstructorId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string TimeZone { get; set; } = "UTC";
    public int MaxCapacity { get; set; }
    public bool IsRecurring { get; set; }
    public RecurringScheduleRequest? RecurringSchedule { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Request model for updating an availability slot.
/// </summary>
public class UpdateAvailabilitySlotRequest
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? TimeZone { get; set; }
    public int MaxCapacity { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Request model for booking a slot.
/// </summary>
public class BookSlotRequest
{
    public int Quantity { get; set; } = 1;
}

/// <summary>
/// Request model for canceling a slot booking.
/// </summary>
public class CancelSlotBookingRequest
{
    public int Quantity { get; set; } = 1;
}

/// <summary>
/// Request model for recurring schedule.
/// </summary>
public class RecurringScheduleRequest
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int Frequency { get; set; } = 1;
    public DateTime? EndDate { get; set; }
} 