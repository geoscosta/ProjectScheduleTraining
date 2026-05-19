using MediatR;

namespace ProjectScheduleTraining.Application.Schedules.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para cancelamento de um horário na agenda.
    /// </summary>
    public record CancelScheduleCommand(Guid Id) : IRequest;
}
