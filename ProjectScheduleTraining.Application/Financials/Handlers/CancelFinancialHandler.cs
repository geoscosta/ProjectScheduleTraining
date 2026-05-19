using MediatR;
using ProjectScheduleTraining.Application.Financials.Commands;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Financials.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de cancelamento de uma cobrança financeira.
    /// Valida a existência e o status da cobrança antes de cancelar.
    /// </summary>
    public class CancelFinancialHandler : IRequestHandler<CancelFinancialCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelFinancialHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de cancelamento da cobrança financeira.
        /// Lança exceção caso a cobrança não seja encontrada, já esteja paga ou cancelada.
        /// </summary>
        public async Task Handle(
            CancelFinancialCommand request,
            CancellationToken cancellationToken)
        {
            var financial = await _unitOfWork.Financials
                .GetByIdAsync(request.Id, cancellationToken);

            if (financial is null)
                throw new DomainException(
                    "Cobrança não encontrada.",
                    "FINANCIAL_NOT_FOUND");

            if (financial.Status == FinancialStatus.Paid)
                throw new DomainException(
                    "Não é possível cancelar uma cobrança já paga.",
                    "FINANCIAL_ALREADY_PAID");

            if (financial.Status == FinancialStatus.Cancelled)
                throw new DomainException(
                    "Cobrança já está cancelada.",
                    "FINANCIAL_ALREADY_CANCELLED");

            financial.Status = FinancialStatus.Cancelled;

            _unitOfWork.Financials.Update(financial);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
