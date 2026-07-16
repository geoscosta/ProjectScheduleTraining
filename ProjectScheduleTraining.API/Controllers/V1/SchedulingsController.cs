using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.Schedulings.Commands;
using ProjectScheduleTraining.Application.Schedulings.DTOs;
using ProjectScheduleTraining.Application.Schedulings.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1
{
    /// <summary>
    /// Controller responsável por expor os endpoints de gerenciamento de agendamentos.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class SchedulingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SchedulingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna um agendamento pelo seu identificador único.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SchedulingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSchedulingByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Retorna todos os agendamentos de um aluno.
        /// </summary>
        [HttpGet("student/{studentId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<SchedulingSummaryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStudentId(Guid studentId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetSchedulingsByStudentIdQuery(studentId),
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Retorna todos os agendamentos de um horário específico.
        /// Utilizado para controle de presença em uma turma.
        /// </summary>
        [HttpGet("schedule/{scheduleId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<SchedulingSummaryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByScheduleId(Guid scheduleId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetSchedulingsByScheduleIdQuery(scheduleId),
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Cria um novo agendamento no sistema.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(SchedulingResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateSchedulingCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Registra a presença de um aluno em uma aula.
        /// </summary>
        [HttpPatch("{id:guid}/checkin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CheckIn(
            Guid id,
            [FromBody] CheckInSchedulingCommand command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command with { Id = id }, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Registra a falta justificada de um aluno em uma aula.
        /// </summary>
        [HttpPatch("{id:guid}/justify-absence")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> JustifyAbsence(
            Guid id,
            [FromBody] JustifyAbsenceCommand command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(command with { Id = id }, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Cancela um agendamento no sistema.
        /// Libera automaticamente a vaga no horário correspondente.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CancelSchedulingCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
