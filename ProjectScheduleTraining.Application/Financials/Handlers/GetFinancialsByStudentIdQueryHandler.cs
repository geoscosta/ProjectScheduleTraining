using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;
using ProjectScheduleTraining.Application.Financials.Mappers;
using ProjectScheduleTraining.Application.Financials.Queries;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Financials.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de listagem de cobranças de um aluno.
    /// </summary>
    public class GetFinancialsByStudentIdQueryHandler : IRequestHandler<GetFinancialsByStudentIdQuery, IEnumerable<FinancialSummaryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFinancialsByStudentIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de listagem de cobranças do aluno.
        /// Retorna todas as cobranças ordenadas da mais recente para a mais antiga.
        /// </summary>
        public async Task<IEnumerable<FinancialSummaryResponse>> Handle(
            GetFinancialsByStudentIdQuery request,
            CancellationToken cancellationToken)
        {
            var financials = await _unitOfWork.Financials
                .GetByStudentIdAsync(request.StudentId, cancellationToken);

            return financials.Select(FinancialMapper.ToSummaryResponse);
        }
    }
}
