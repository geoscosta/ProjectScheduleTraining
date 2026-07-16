using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.Plans.Commands;
using ProjectScheduleTraining.Application.Plans.DTOs;
using ProjectScheduleTraining.Application.Plans.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1;

/// <summary>
/// Controller responsável por expor os endpoints de gerenciamento de planos.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class PlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retorna todos os planos ativos do sistema.
    /// Acessível por qualquer usuário autenticado (consulta pública interna).
    /// </summary>
    [HttpGet]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(IEnumerable<PlanSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllPlansQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retorna um plano pelo seu identificador único.
    /// Acessível por qualquer usuário autenticado.
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "AllRoles")]
    [ProducesResponseType(typeof(PlanResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPlanByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Cria um novo plano no sistema.
    /// Restrito a Administradores.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(PlanResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
        [FromBody] CreatePlanCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Atualiza os dados de um plano existente.
    /// Restrito a Administradores.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(PlanResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdatePlanCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Desativa um plano no sistema.
    /// Restrito a Administradores.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeactivatePlanCommand(id), cancellationToken);
        return NoContent();
    }
}
