using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.Schedules.Commands;
using ProjectScheduleTraining.Application.Schedules.DTOs;
using ProjectScheduleTraining.Application.Schedules.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1;

/// <summary>
/// Controller responsável por expor os endpoints de gerenciamento de horários.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class SchedulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SchedulesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(ScheduleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetScheduleByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("date/{date}")]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDate(DateTime date, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSchedulesByDateQuery(date), cancellationToken);
        return Ok(result);
    }

    [HttpGet("period")]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(IEnumerable<ScheduleSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetByPeriod(
        [FromQuery] DateTime start,
        [FromQuery] DateTime end,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSchedulesByPeriodQuery(start, end), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Cria um novo horário na agenda. Restrito a Admin e Trainer.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "TrainerOrAdmin")]
    [ProducesResponseType(typeof(ScheduleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateScheduleCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Bloqueia um horário na agenda. Restrito a Admin e Trainer.
    /// </summary>
    [HttpPatch("{id:guid}/block")]
    [Authorize(Policy = "TrainerOrAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Block(
        Guid id,
        [FromBody] BlockScheduleCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command with { Id = id }, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Cancela um horário na agenda. Restrito a Admin e Trainer.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "TrainerOrAdmin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelScheduleCommand(id), cancellationToken);
        return NoContent();
    }
}
