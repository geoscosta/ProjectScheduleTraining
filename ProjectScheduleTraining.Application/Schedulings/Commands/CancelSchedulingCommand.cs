using MediatR;

namespace ProjectScheduleTraining.Application.Schedulings.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para cancelamento de um agendamento no sistema.
    /// </summary>
    public record CancelSchedulingCommand(Guid Id) : IRequest;
}
