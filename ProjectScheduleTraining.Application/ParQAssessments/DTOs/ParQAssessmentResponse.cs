namespace ProjectScheduleTraining.Application.ParQAssessments.DTOs
{
    /// <summary>
    /// DTO de resposta da avaliação PAR-Q.
    /// </summary>
    public record ParQAssessmentResponse(
        Guid Id,
        Guid StudentId,
        DateTime AssessmentDate,
        bool Question1,
        bool Question2,
        bool Question3,
        bool Question4,
        bool Question5,
        bool Question6,
        bool Question7,
        bool IsCleared,
        string? Notes,
        DateTime CreatedAt);
}
