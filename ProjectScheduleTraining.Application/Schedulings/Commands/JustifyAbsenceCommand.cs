using MediatR;

namespace ProjectScheduleTraining.Application.Schedulings.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para registro de falta justificada de um aluno em uma aula.
    /// </summary>
    public record JustifyAbsenceCommand(
        Guid Id,
        string Reason) : IRequest;
}
