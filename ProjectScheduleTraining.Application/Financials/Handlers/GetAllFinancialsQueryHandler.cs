using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;
using ProjectScheduleTraining.Application.Financials.Mappers;
using ProjectScheduleTraining.Application.Financials.Queries;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.Financials.Handlers;

/// <summary>
/// Handler responsável por retornar todas as cobranças do sistema.
/// </summary>
public class GetAllFinancialsQueryHandler
    : IRequestHandler<GetAllFinancialsQuery, IEnumerable<FinancialResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllFinancialsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<FinancialResponse>> Handle(
        GetAllFinancialsQuery request,
        CancellationToken cancellationToken)
    {
        var financials = await _unitOfWork.Financials
            .GetAllAsync(cancellationToken);

        return financials
            .OrderByDescending(f => f.DueDate)
            .Select(FinancialMapper.ToResponse);
    }
}