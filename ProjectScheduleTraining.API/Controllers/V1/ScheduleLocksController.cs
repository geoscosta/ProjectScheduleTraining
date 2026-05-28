using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.ScheduleLocks.Commands;
using ProjectScheduleTraining.Application.ScheduleLocks.DTOs;
using ProjectScheduleTraining.Application.ScheduleLocks.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1
{
    /// <summary>
    /// Controller responsável por expor os endpoints de trancamentos de agenda.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ScheduleLocksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ScheduleLocksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna todos os trancamentos de um aluno.
        /// </summary>
        [HttpGet("student/{studentId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<ScheduleLockResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStudent(
            Guid studentId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetScheduleLocksByStudentIdQuery(studentId),
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Retorna o trancamento ativo de um aluno se existir.
        /// </summary>
        [HttpGet("student/{studentId:guid}/active")]
        [ProducesResponseType(typeof(ScheduleLockResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActive(
            Guid studentId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetActiveScheduleLockQuery(studentId),
                cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Solicita um trancamento de agenda.
        /// Apenas alunos com planos fidelidade têm direito.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ScheduleLockResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateScheduleLockCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(
                nameof(GetActive),
                new { studentId = result.StudentId },
                result);
        }

        /// <summary>
        /// Aprova um trancamento de agenda.
        /// Apenas administradores podem aprovar.
        /// </summary>
        [HttpPatch("{id:guid}/approve")]
        [ProducesResponseType(typeof(ScheduleLockResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Approve(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ApproveScheduleLockCommand(id),
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Rejeita um trancamento de agenda.
        /// </summary>
        [HttpPatch("{id:guid}/reject")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Reject(
            Guid id,
            [FromBody] RejectScheduleLockCommand command,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                command with { Id = id },
                cancellationToken);
            return NoContent();
        }
    }
}
