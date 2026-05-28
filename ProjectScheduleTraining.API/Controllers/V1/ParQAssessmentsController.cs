using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.ParQAssessments.Commands;
using ProjectScheduleTraining.Application.ParQAssessments.DTOs;
using ProjectScheduleTraining.Application.ParQAssessments.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1
{
    /// <summary>
    /// Controller responsável por expor os endpoints de avaliações PAR-Q.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ParQAssessmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParQAssessmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna a avaliação PAR-Q mais recente de um aluno.
        /// </summary>
        [HttpGet("student/{studentId:guid}/latest")]
        [ProducesResponseType(typeof(ParQAssessmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLatest(
            Guid studentId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetLatestParQAssessmentQuery(studentId),
                cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Registra a avaliação PAR-Q de um aluno.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ParQAssessmentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(
            [FromBody] CreateParQAssessmentCommand command,
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
