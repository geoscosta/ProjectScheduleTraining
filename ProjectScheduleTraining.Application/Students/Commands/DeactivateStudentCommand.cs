using MediatR;

namespace ProjectScheduleTraining.Application.Students.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para inativação de um aluno no sistema.
    /// </summary>
    public record DeactivateStudentCommand(Guid Id) : IRequest;
}
