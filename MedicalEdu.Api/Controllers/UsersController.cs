using Microsoft.AspNetCore.Mvc;
using MedicalEdu.Domain.DataAccess.Services;
using MedicalEdu.Domain.Aggregates;

namespace MedicalEdu.Api.Controllers;

/// <summary>
/// Controller for user-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    /// <summary>
    /// Gets a user by their unique identifier.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user if found; otherwise, 404 Not Found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserAggregate>> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAggregateAsync(id, cancellationToken);
        
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created user.</returns>
    [HttpPost]
    public async Task<ActionResult<UserAggregate>> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.CreateUserAsync(
                request.Name, 
                request.Email, 
                request.Password, 
                request.Role, 
                cancellationToken);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Confirms a user's email address.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="request">The email confirmation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPost("{id}/confirm-email")]
    public async Task<ActionResult> ConfirmEmail(Guid id, [FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var success = await _userService.ConfirmEmailAsync(id, request.Token, cancellationToken);
        
        if (!success)
        {
            return BadRequest("Invalid or expired confirmation token.");
        }

        return Ok();
    }

    /// <summary>
    /// Changes a user's password.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="request">The password change request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPost("{id}/change-password")]
    public async Task<ActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var success = await _userService.ChangePasswordAsync(id, request.NewPassword, cancellationToken);
        
        if (!success)
        {
            return BadRequest("Failed to change password.");
        }

        return Ok();
    }
}

/// <summary>
/// Request model for creating a new user.
/// </summary>
public class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}

/// <summary>
/// Request model for confirming email.
/// </summary>
public class ConfirmEmailRequest
{
    public string Token { get; set; } = string.Empty;
}

/// <summary>
/// Request model for changing password.
/// </summary>
public class ChangePasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
} 