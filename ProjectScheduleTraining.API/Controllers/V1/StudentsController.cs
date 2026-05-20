using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.Students.Commands;
using ProjectScheduleTraining.Application.Students.DTOs;
using ProjectScheduleTraining.Application.Students.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1
{
    /// <summary>
    /// Controller responsável por expor os endpoints de gerenciamento de alunos.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna todos os alunos ativos do sistema.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentSummaryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllStudentsQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Retorna um aluno pelo seu identificador único.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetStudentByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Cria um novo aluno no sistema.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateStudentCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Atualiza os dados de um aluno existente.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateStudentCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command with { Id = id }, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Bloqueia um aluno no sistema.
        /// </summary>
        [HttpPatch("{id:guid}/block")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Block(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new BlockStudentCommand(id), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Desbloqueia um aluno no sistema.
        /// </summary>
        [HttpPatch("{id:guid}/unblock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Unblock(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UnblockStudentCommand(id), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Inativa um aluno no sistema.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeactivateStudentCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
