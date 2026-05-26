using MediatR;
using ProjectScheduleTraining.Application.Financials.DTOs;

namespace ProjectScheduleTraining.Application.Financials.Queries;

/// <summary>
/// Query responsável por transportar os dados necessários
/// para geração do relatório financeiro de um mês específico.
/// </summary>
public record GetFinancialReportQuery(int Month, int Year)
    : IRequest<FinancialReportResponse>;