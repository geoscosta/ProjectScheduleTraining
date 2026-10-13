using MediatR;
using ProjectScheduleTraining.Application.Schedules.DTOs;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Schedules.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para criação de um novo horário na agenda.
    /// </summary>
    public record CreateScheduleCommand(
        DateTime Date,
        SessionType SessionType,
        TimeSpan StartTime) : IRequest<ScheduleResponse>;
}
