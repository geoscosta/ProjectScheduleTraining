using MediatR;

namespace ProjectScheduleTraining.Application.Schedulings.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para registro de presença de um aluno em uma aula.
    /// </summary>
    public record CheckInSchedulingCommand(
        Guid Id,
        string? TrainerNotes) : IRequest;
}
