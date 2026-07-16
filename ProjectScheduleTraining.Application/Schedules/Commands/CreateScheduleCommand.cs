using MediatR;
using ProjectScheduleTraining.Application.Schedules.DTOs;

namespace ProjectScheduleTraining.Application.Schedules.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para criação de um novo horário na agenda.
    /// </summary>
    public record CreateScheduleCommand(
        DateTime Date,
        TimeSpan StartTime) : IRequest<ScheduleResponse>;
}
