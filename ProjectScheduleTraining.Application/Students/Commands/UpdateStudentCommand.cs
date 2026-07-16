using MediatR;
using ProjectScheduleTraining.Application.Students.DTOs;

namespace ProjectScheduleTraining.Application.Students.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para atualização de um aluno existente no sistema.
    /// </summary>
    public record UpdateStudentCommand(
        Guid Id,
        string Name,
        string Email,
        string Phone,
        string? Address,
        string? EmergencyContact) : IRequest<StudentResponse>;
}
