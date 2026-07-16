using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Students.DTOs
{
    /// <summary>
    /// DTO de resposta completa do aluno.
    /// Utilizado em operações de busca detalhada.
    /// </summary>
    public record StudentResponse(
        Guid Id,
        string Name,
        string Cpf,
        string Email,
        string Phone,
        DateTime BirthDate,
        string? Address,
        string? EmergencyContact,
        string? PhotoUrl,
        string? InternalNotes,
        StudentStatus Status,
        DateTime StartDate,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
