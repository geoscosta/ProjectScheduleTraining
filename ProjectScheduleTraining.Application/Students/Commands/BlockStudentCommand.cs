using MediatR;

namespace ProjectScheduleTraining.Application.Students.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para bloqueio de um aluno no sistema.
    /// </summary>
    public record BlockStudentCommand(Guid Id) : IRequest;
}
