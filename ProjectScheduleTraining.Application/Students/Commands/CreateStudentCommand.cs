using MediatR;
using ProjectScheduleTraining.Application.Students.DTOs;

namespace ProjectScheduleTraining.Application.Students.Commands
{
    /// <summary>
    /// Command responsável por transportar os dados necessários
    /// para criação de um novo aluno no sistema.
    /// </summary>
    public record CreateStudentCommand(
        string Name,
        string Cpf,
        string Email,
        string Phone,
        DateTime BirthDate,
        string? Address,
        string? EmergencyContact) : IRequest<StudentResponse>;
}
