using MediatR;
using ProjectScheduleTraining.Application.Plans.DTOs;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Plans.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para criação de um novo plano no sistema.
    /// </summary>
    public record CreatePlanCommand(
        string Name,
        PlanType Type,
        WeeklyFrequency WeeklyFrequency,
        int DurationMonths,
        decimal Price,
        string? Description) : IRequest<PlanResponse>;
}
