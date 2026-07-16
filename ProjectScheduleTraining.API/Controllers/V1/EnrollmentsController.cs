using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.Enrollments.Commands;
using ProjectScheduleTraining.Application.Enrollments.DTOs;
using ProjectScheduleTraining.Application.Enrollments.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1;

/// <summary>
/// Controller responsável por expor os endpoints de gerenciamento de matrículas.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnrollmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(EnrollmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEnrollmentByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("student/{studentId:guid}")]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(EnrollmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByStudentId(Guid studentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetEnrollmentByStudentIdQuery(studentId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ReceptionistOrAdmin")]
    [ProducesResponseType(typeof(EnrollmentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateEnrollmentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id:guid}/renew")]
    [Authorize(Policy = "ReceptionistOrAdmin")]
    [ProducesResponseType(typeof(EnrollmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Renew(
        Guid id,
        [FromBody] RenewEnrollmentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelEnrollmentCommand(id), cancellationToken);
        return NoContent();
    }
}
