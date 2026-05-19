using MediatR;
using ProjectScheduleTraining.Application.Financials.Commands;
using ProjectScheduleTraining.Application.Financials.DTOs;
using ProjectScheduleTraining.Application.Financials.Mappers;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Financials.Handlers
{
    /// <summary>
    /// Handler responsável por processar o comando de registro de pagamento.
    /// Valida a existência e o status da cobrança antes de registrar o pagamento.
    /// </summary>
    public class RegisterPaymentHandler : IRequestHandler<RegisterPaymentCommand, FinancialResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegisterPaymentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de registro de pagamento.
        /// Lança exceção caso a cobrança não seja encontrada ou já esteja paga.
        /// </summary>
        public async Task<FinancialResponse> Handle(
            RegisterPaymentCommand request,
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
                    "Cobrança já foi paga.",
                    "FINANCIAL_ALREADY_PAID");

            if (financial.Status == FinancialStatus.Cancelled)
                throw new DomainException(
                    "Não é possível registrar pagamento de uma cobrança cancelada.",
                    "FINANCIAL_CANCELLED");

            financial.Status = FinancialStatus.Paid;
            financial.PaymentDate = DateTime.UtcNow;
            financial.PaymentProof = request.PaymentProof?.Trim();

            _unitOfWork.Financials.Update(financial);
            await _unitOfWork.CommitAsync(cancellationToken);

            return FinancialMapper.ToResponse(financial);
        }
    }
}
