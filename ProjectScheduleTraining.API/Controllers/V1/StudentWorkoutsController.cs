using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.StudentWorkouts.Commands;
using ProjectScheduleTraining.Application.StudentWorkouts.DTOs;
using ProjectScheduleTraining.Application.StudentWorkouts.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1
{
    /// <summary>
    /// Controller responsável por expor os endpoints de treinos personalizados.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StudentWorkoutsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentWorkoutsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna o treino ativo de um aluno com todos os exercícios.
        /// </summary>
        [HttpGet("student/{studentId:guid}/active")]
        [ProducesResponseType(typeof(StudentWorkoutResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActive(
            Guid studentId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetActiveWorkoutByStudentIdQuery(studentId),
                cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Retorna todos os treinos de um aluno.
        /// </summary>
        [HttpGet("student/{studentId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<StudentWorkoutSummaryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByStudent(
            Guid studentId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetAllWorkoutsByStudentIdQuery(studentId),
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Retorna todos os treinos criados por um professor.
        /// </summary>
        [HttpGet("trainer/{trainerId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<StudentWorkoutSummaryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByTrainer(
            Guid trainerId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetWorkoutsByTrainerIdQuery(trainerId),
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Cria um novo treino personalizado para um aluno.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(StudentWorkoutResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateStudentWorkoutCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(
                nameof(GetActive),
                new { studentId = result.StudentId },
                result);
        }

        /// <summary>
        /// Atualiza um treino existente.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(StudentWorkoutResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateStudentWorkoutCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                command with { Id = id },
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Desativa um treino.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deactivate(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _mediator.Send(
                new DeactivateStudentWorkoutCommand(id),
                cancellationToken);
            return NoContent();
        }
    }
}
