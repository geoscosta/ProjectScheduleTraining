using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;
using ProjectScheduleTraining.Application.Financials.Mappers;
using ProjectScheduleTraining.Application.Financials.Queries;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Financials.Handlers
{
    /// <summary>
    /// Handler responsável por processar a query de busca de uma cobrança pelo identificador.
    /// Lança exceção caso a cobrança não seja encontrada.
    /// </summary>
    public class GetFinancialByIdQueryHandler : IRequestHandler<GetFinancialByIdQuery, FinancialResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFinancialByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa a query de busca da cobrança pelo identificador único.
        /// Lança exceção caso a cobrança não seja encontrada.
        /// </summary>
        public async Task<FinancialResponse> Handle(
            GetFinancialByIdQuery request,
            CancellationToken cancellationToken)
        {
            var financial = await _unitOfWork.Financials
                .GetByIdAsync(request.Id, cancellationToken);

            if (financial is null)
                throw new DomainException(
                    "Cobrança não encontrada.",
                    "FINANCIAL_NOT_FOUND");

            return FinancialMapper.ToResponse(financial);
        }
    }
}
