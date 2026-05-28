using MediatR;
using ProjectScheduleTraining.Application.ParQAssessments.DTOs;

namespace ProjectScheduleTraining.Application.ParQAssessments.Commands
{
    /// <summary>
    /// Command para registrar a avaliação PAR-Q de um aluno.
    /// </summary>
    public record CreateParQAssessmentCommand(
        Guid StudentId,
        bool Question1,
        bool Question2,
        bool Question3,
        bool Question4,
        bool Question5,
        bool Question6,
        bool Question7,
        string? Notes) : IRequest<ParQAssessmentResponse>;
}
