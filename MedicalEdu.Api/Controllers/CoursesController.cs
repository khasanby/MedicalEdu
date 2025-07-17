using MediatR;
using Microsoft.AspNetCore.Mvc;
using MedicalEdu.Application.Courses.Create;
using MedicalEdu.Application.Courses.Update;
using MedicalEdu.Application.Courses.GetAll;
using MedicalEdu.Application.Courses.GetById;
using MedicalEdu.Application.Common.Results;

namespace MedicalEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CreateCourseResponse>> CreateCourse(
        [FromBody] CreateCourseCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.ValidationErrors?.Any() == true)
                return BadRequest(new { errors = result.ValidationErrors });
            
            if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
                return NotFound(new { error = result.Error });
            
            if (result.Error?.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) == true)
                return Unauthorized(new { error = result.Error });
            
            return BadRequest(new { error = result.Error });
        }

        return CreatedAtAction(nameof(GetCourseById), new { id = result.Value!.CourseId }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateCourseResponse>> UpdateCourse(
        Guid id,
        [FromBody] UpdateCourseCommand command,
        CancellationToken cancellationToken)
    {
        // Ensure the command has the correct course ID
        command = command with { CourseId = id };
        
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.ValidationErrors?.Any() == true)
                return BadRequest(new { errors = result.ValidationErrors });
            
            if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
                return NotFound(new { error = result.Error });
            
            if (result.Error?.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) == true)
                return Unauthorized(new { error = result.Error });
            
            return BadRequest(new { error = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<GetAllCoursesResponse>> GetAllCourses(
        [FromQuery] GetAllCoursesQuery query,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetCourseByIdResponse>> GetCourseById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetCourseByIdQuery(id);
        var response = await _mediator.Send(query, cancellationToken);
        
        if (response == null)
            return NotFound();

        return Ok(response);
    }
} 