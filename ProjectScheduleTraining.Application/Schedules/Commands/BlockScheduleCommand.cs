using MediatR;

namespace ProjectScheduleTraining.Application.Schedules.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para bloqueio de um horário na agenda.
    /// </summary>
    public record BlockScheduleCommand(
        Guid Id,
        string? Notes) : IRequest;
}
