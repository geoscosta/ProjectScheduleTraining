using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;
using ProjectScheduleTraining.Application.Financials.Queries;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Financials.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de relatório financeiro.
    /// Consolida totais de recebimentos, pendências e inadimplência do mês.
    /// </summary>
    public class GetFinancialReportQueryHandler
        : IRequestHandler<GetFinancialReportQuery, FinancialReportResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFinancialReportQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de relatório financeiro.
        /// Calcula totais de recebimentos, pendências e vencimentos do mês informado.
        /// </summary>
        public async Task<FinancialReportResponse> Handle(
            GetFinancialReportQuery request,
            CancellationToken cancellationToken)
        {
            /// Busca o total recebido no mês via repositório otimizado.
            var totalReceived = await _unitOfWork.Financials
                .GetTotalReceivedByMonthAsync(request.Year, request.Month, cancellationToken);

            /// Busca todas as cobranças para calcular pendentes e vencidas do mês.
            var allFinancials = await _unitOfWork.Financials
                .GetAllAsync(cancellationToken);

            var monthFinancials = allFinancials
                .Where(f => f.DueDate.Month == request.Month &&
                            f.DueDate.Year == request.Year)
                .ToList();

            /// Calcula totais de cobranças pendentes do mês.
            var pendingFinancials = monthFinancials
                .Where(f => f.Status == FinancialStatus.Pending)
                .ToList();

            /// Calcula totais de cobranças vencidas do mês.
            var overdueFinancials = monthFinancials
                .Where(f => f.Status == FinancialStatus.Overdue ||
                           (f.Status == FinancialStatus.Pending &&
                            f.DueDate < DateTime.UtcNow))
                .ToList();

            /// Calcula total de cobranças pagas no mês.
            var receivedFinancials = monthFinancials
                .Where(f => f.Status == FinancialStatus.Paid)
                .ToList();

            return new FinancialReportResponse(
                Month: request.Month,
                Year: request.Year,
                TotalReceived: receivedFinancials.Sum(f => f.Amount),
                TotalPending: pendingFinancials.Sum(f => f.Amount),
                TotalOverdue: overdueFinancials.Sum(f => f.Amount),
                CountReceived: receivedFinancials.Count,
                CountPending: pendingFinancials.Count,
                CountOverdue: overdueFinancials.Count);
        }
    }
}
