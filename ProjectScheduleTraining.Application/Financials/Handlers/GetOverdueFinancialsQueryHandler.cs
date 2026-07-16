using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;
using ProjectScheduleTraining.Application.Financials.Mappers;
using ProjectScheduleTraining.Application.Financials.Queries;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Financials.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de listagem de cobranças vencidas.
    /// Utilizado para controle de inadimplência e bloqueio automático de alunos.
    /// </summary>
    public class GetOverdueFinancialsQueryHandler : IRequestHandler<GetOverdueFinancialsQuery, IEnumerable<FinancialSummaryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOverdueFinancialsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de listagem de cobranças vencidas.
        /// Retorna todas as cobranças com status vencido ou pendentes com data ultrapassada.
        /// </summary>
        public async Task<IEnumerable<FinancialSummaryResponse>> Handle(
            GetOverdueFinancialsQuery request,
            CancellationToken cancellationToken)
        {
            var financials = await _unitOfWork.Financials
                .GetOverdueAsync(cancellationToken);

            return financials.Select(FinancialMapper.ToSummaryResponse);
        }
    }
}
