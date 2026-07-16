using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.ScheduleLocks.Commands;
using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;
using ProjectScheduleTraining.Application.ScheduleLocks.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1;

/// <summary>
/// Controller responsável por expor os endpoints de trancamentos de agenda.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ScheduleLocksController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScheduleLocksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("student/{studentId:guid}")]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleLockResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStudent(Guid studentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetScheduleLocksByStudentIdQuery(studentId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("student/{studentId:guid}/active")]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(ScheduleLockResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActive(Guid studentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetActiveScheduleLockQuery(studentId), cancellationToken);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(ScheduleLockResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateScheduleLockCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetActive), new { studentId = result.StudentId }, result);
    }

    /// <summary>
    /// Aprova um trancamento de agenda.
    /// Restrito a Administradores — operação de aprovação administrativa.
    /// </summary>
    [HttpPatch("{id:guid}/approve")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ScheduleLockResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ApproveScheduleLockCommand(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Rejeita um trancamento de agenda.
    /// Restrito a Administradores.
    /// </summary>
    [HttpPatch("{id:guid}/reject")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Reject(
        Guid id,
        [FromBody] RejectScheduleLockCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command with { Id = id }, cancellationToken);
        return NoContent();
    }
}
