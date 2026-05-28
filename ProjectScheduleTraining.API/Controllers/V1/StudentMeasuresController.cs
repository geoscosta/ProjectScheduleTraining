using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.StudentMeasures.Commands;
using ProjectScheduleTraining.Application.StudentMeasures.DTOs;
using ProjectScheduleTraining.Application.StudentMeasures.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1
{
    /// <summary>
    /// Controller responsável por expor os endpoints de medidas corporais dos alunos.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StudentMeasuresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentMeasuresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna todas as medidas corporais de um aluno.
        /// </summary>
        [HttpGet("student/{studentId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<StudentMeasureResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStudentId(
            Guid studentId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetStudentMeasuresByStudentIdQuery(studentId),
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Retorna a medida corporal mais recente de um aluno.
        /// </summary>
        [HttpGet("student/{studentId:guid}/latest")]
        [ProducesResponseType(typeof(StudentMeasureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLatest(
            Guid studentId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetLatestStudentMeasureQuery(studentId),
                cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Registra novas medidas corporais para um aluno.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(StudentMeasureResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateStudentMeasureCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(
                nameof(GetLatest),
                new { studentId = result.StudentId },
                result);
        }
    }
}
