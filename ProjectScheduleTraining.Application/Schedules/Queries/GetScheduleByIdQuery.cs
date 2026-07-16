using MediatR;
using ProjectScheduleTraining.Application.Schedules.DTOs;

namespace ProjectScheduleTraining.Application.Schedules.Queries
{
    /// <summary>
    /// Query responsável por transportar os dados necessários
    /// para busca de um horário pelo seu identificador único.
    /// </summary>
    public record GetScheduleByIdQuery(Guid Id) : IRequest<ScheduleResponse>;
}
