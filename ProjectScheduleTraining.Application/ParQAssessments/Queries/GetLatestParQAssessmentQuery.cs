using MediatR;
using ProjectScheduleTraining.Application.ParQAssessments.DTOs;

namespace ProjectScheduleTraining.Application.ParQAssessments.Queries
{
    /// <summary>
    /// Query para buscar a avaliação PAR-Q mais recente de um aluno.
    /// </summary>
    public record GetLatestParQAssessmentQuery(Guid StudentId)
        : IRequest<ParQAssessmentResponse?>;
}
