using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Application.Students.DTOs
{
    /// <summary>
    /// DTO de resposta resumida do aluno.
    /// Utilizado em listagens e buscas gerais.
    /// </summary>
    public record StudentSummaryResponse(
        Guid Id,
        string Name,
        string Email,
        string Phone,
        StudentStatus Status);
}
