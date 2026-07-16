using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.Financials.Commands;
using ProjectScheduleTraining.Application.Financials.DTOs;
using ProjectScheduleTraining.Application.Financials.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1;

/// <summary>
/// Controller responsável por expor os endpoints de gerenciamento financeiro.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class FinancialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FinancialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = "ReceptionistOrAdmin")]
    [ProducesResponseType(typeof(FinancialResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFinancialByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("student/{studentId:guid}")]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(IEnumerable<FinancialSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStudentId(Guid studentId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetFinancialsByStudentIdQuery(studentId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retorna todas as cobranças vencidas do sistema.
    /// Restrito a Admin e Receptionist — dado sensível de inadimplência.
    /// </summary>
    [HttpGet("overdue")]
    [Authorize(Policy = "ReceptionistOrAdmin")]
    [ProducesResponseType(typeof(IEnumerable<FinancialSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOverdue(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOverdueFinancialsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "ReceptionistOrAdmin")]
    [ProducesResponseType(typeof(FinancialResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreateFinancialCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id:guid}/payment")]
    [Authorize(Policy = "ReceptionistOrAdmin")]
    [ProducesResponseType(typeof(FinancialResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RegisterPayment(
        Guid id,
        [FromBody] RegisterPaymentCommand command,
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
        await _mediator.Send(new CancelFinancialCommand(id), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Retorna o relatório financeiro consolidado. Restrito a Administradores.
    /// </summary>
    [HttpGet("report")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(FinancialReportResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReport(
        [FromQuery] int month,
        [FromQuery] int year,
        CancellationToken cancellationToken)
    {
        var currentDate = DateTime.UtcNow;
        var reportMonth = month > 0 ? month : currentDate.Month;
        var reportYear = year > 0 ? year : currentDate.Year;
        var result = await _mediator.Send(new GetFinancialReportQuery(reportMonth, reportYear), cancellationToken);
        return Ok(result);
    }
}
