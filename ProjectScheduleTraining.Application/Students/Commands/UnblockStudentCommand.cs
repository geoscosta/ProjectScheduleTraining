using MediatR;

namespace ProjectScheduleTraining.Application.Students.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para desbloqueio de um aluno no sistema.
    /// </summary>
    public record UnblockStudentCommand(Guid Id) : IRequest;
}
