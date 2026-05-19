using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectScheduleTraining.Application.Financials.Commands;
using ProjectScheduleTraining.Application.Financials.DTOs;
using ProjectScheduleTraining.Application.Financials.Queries;

namespace ProjectScheduleTraining.API.Controllers.V1
{
    /// <summary>
    /// Controller responsável por expor os endpoints de gerenciamento financeiro.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FinancialsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FinancialsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retorna uma cobrança pelo seu identificador único.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(FinancialResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetFinancialByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Retorna todas as cobranças de um aluno.
        /// </summary>
        [HttpGet("student/{studentId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<FinancialSummaryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStudentId(Guid studentId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetFinancialsByStudentIdQuery(studentId),
                cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Retorna todas as cobranças vencidas do sistema.
        /// Utilizado para controle de inadimplência.
        /// </summary>
        [HttpGet("overdue")]
        [ProducesResponseType(typeof(IEnumerable<FinancialSummaryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOverdue(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOverdueFinancialsQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Cria uma nova cobrança financeira no sistema.
        /// </summary>
        [HttpPost]
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

        /// <summary>
        /// Registra o pagamento de uma cobrança financeira.
        /// </summary>
        [HttpPatch("{id:guid}/payment")]
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

        /// <summary>
        /// Cancela uma cobrança financeira no sistema.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CancelFinancialCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
