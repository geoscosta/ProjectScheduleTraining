using MediatR;
using ProjectScheduleTraining.Application.Students.DTOs;
using ProjectScheduleTraining.Domain.Enums;

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
        string? Profession,
        MaritalStatus? MaritalStatus,
        string? IdentityDocument,
        string? Street,
        string? AddressNumber,
        string? Complement,
        string? District,
        string? City,
        string? State,
        string? ZipCode,
        string? GuardianName,
        string? GuardianCpf,
        string? EmergencyContact,
        bool ImageRightsAccepted,
        bool InternalRegulationAccepted)
        : IRequest<StudentResponse>;
}
