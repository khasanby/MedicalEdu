using MedicalEdu.Domain.DataAccess.Repositories;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MedicalEdu.Api.Controllers;

/// <summary>
/// Controller for booking-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IAvailabilitySlotRepository _availabilitySlotRepository;
    private readonly IUserRepository _userRepository;

    public BookingsController(
        IBookingRepository bookingRepository,
        IAvailabilitySlotRepository availabilitySlotRepository,
        IUserRepository userRepository)
    {
        _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(bookingRepository));
        _availabilitySlotRepository = availabilitySlotRepository ?? throw new ArgumentNullException(nameof(availabilitySlotRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    /// <summary>
    /// Gets a booking by its unique identifier.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking == null)
        {
            return NotFound();
        }

        return Ok(booking);
    }

    /// <summary>
    /// Gets all bookings with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings(
        [FromQuery] Guid? userId = null,
        [FromQuery] Guid? instructorId = null,
        [FromQuery] BookingStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var bookings = await _bookingRepository.GetAllAsync(userId, instructorId, status, cancellationToken);
        return Ok(bookings);
    }

    /// <summary>
    /// Creates a new booking.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate availability slot exists and is available
            var availabilitySlot = await _availabilitySlotRepository.GetByIdAsync(request.AvailabilitySlotId, cancellationToken);
            if (availabilitySlot == null)
            {
                return BadRequest("Availability slot not found.");
            }

            // Validate user exists
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Create value objects
            var amount = Money.Create(request.Amount, Currency.Create(request.CurrencyCode));
            var promoCode = !string.IsNullOrEmpty(request.PromoCode) ? PromoCode.Create(request.PromoCode) : null;

            // Create booking entity
            var booking = new Booking(
                id: Guid.NewGuid(),
                userId: request.UserId,
                instructorId: request.InstructorId,
                availabilitySlotId: request.AvailabilitySlotId,
                amount: amount,
                promoCode: promoCode,
                notes: request.Notes);

            // Add to repository
            await _bookingRepository.AddAsync(booking, cancellationToken);
            await _bookingRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
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
    /// Confirms a booking.
    /// </summary>
    [HttpPost("{id}/confirm")]
    public async Task<ActionResult> ConfirmBooking(Guid id, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking == null)
        {
            return NotFound();
        }

        try
        {
            booking.Confirm();
            await _bookingRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Cancels a booking.
    /// </summary>
    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> CancelBooking(Guid id, [FromBody] CancelBookingRequest? request = null, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking == null)
        {
            return NotFound();
        }

        try
        {
            booking.Cancel(request?.Reason);
            await _bookingRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Completes a booking.
    /// </summary>
    [HttpPost("{id}/complete")]
    public async Task<ActionResult> CompleteBooking(Guid id, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking == null)
        {
            return NotFound();
        }

        try
        {
            booking.Complete();
            await _bookingRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// No-shows a booking.
    /// </summary>
    [HttpPost("{id}/no-show")]
    public async Task<ActionResult> NoShowBooking(Guid id, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking == null)
        {
            return NotFound();
        }

        try
        {
            booking.MarkAsNoShow();
            await _bookingRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates booking notes.
    /// </summary>
    [HttpPut("{id}/notes")]
    public async Task<ActionResult> UpdateBookingNotes(Guid id, [FromBody] UpdateBookingNotesRequest request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking == null)
        {
            return NotFound();
        }

        try
        {
            booking.UpdateNotes(request.Notes);
            await _bookingRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gets bookings for a specific user.
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetUserBookings(Guid userId, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetByUserIdAsync(userId, cancellationToken);
        return Ok(bookings);
    }

    /// <summary>
    /// Gets bookings for a specific instructor.
    /// </summary>
    [HttpGet("instructor/{instructorId}")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetInstructorBookings(Guid instructorId, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetByInstructorIdAsync(instructorId, cancellationToken);
        return Ok(bookings);
    }
}

/// <summary>
/// Request model for creating a new booking.
/// </summary>
public class CreateBookingRequest
{
    public Guid UserId { get; set; }
    public Guid InstructorId { get; set; }
    public Guid AvailabilitySlotId { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = "USD";
    public string? PromoCode { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Request model for canceling a booking.
/// </summary>
public class CancelBookingRequest
{
    public string? Reason { get; set; }
}

/// <summary>
/// Request model for updating booking notes.
/// </summary>
public class UpdateBookingNotesRequest
{
    public string? Notes { get; set; }
}