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
        string? PhotoUrl,
        bool RegistrationFeePaid,
        DateTime? HealthCertificateExpiresAt,
        bool ImageRightsAccepted,
        bool InternalRegulationAccepted,
        StudentStatus Status,
        DateTime StartDate,
        DateTime CreatedAt,
        DateTime UpdatedAt);
}
