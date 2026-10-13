using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;

namespace ProjectScheduleTraining.Application.Financials.Queries;

public record GetAllFinancialsQuery() : IRequest<IEnumerable<FinancialResponse>>;