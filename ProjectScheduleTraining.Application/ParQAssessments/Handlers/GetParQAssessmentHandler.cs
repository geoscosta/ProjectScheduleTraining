using MediatR;
using ProjectScheduleTraining.Application.ParQAssessments.DTOs;
using ProjectScheduleTraining.Application.ParQAssessments.Queries;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.ParQAssessments.Handlers
{
    /// <summary>
    /// Handler responsável por buscar a avaliação PAR-Q de um aluno.
    /// </summary>
    public class GetParQAssessmentHandler
        : IRequestHandler<GetLatestParQAssessmentQuery, ParQAssessmentResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetParQAssessmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retorna a avaliação PAR-Q mais recente do aluno.
        /// Retorna null se o aluno ainda não realizou a avaliação.
        /// </summary>
        public async Task<ParQAssessmentResponse?> Handle(
            GetLatestParQAssessmentQuery request,
            CancellationToken cancellationToken)
        {
            var assessment = await _unitOfWork.ParQAssessments
                .GetLatestByStudentIdAsync(request.StudentId, cancellationToken);

            return assessment is null ? null : MapToResponse(assessment);
        }

        private static ParQAssessmentResponse MapToResponse(ParQAssessment assessment)
            => new(
                assessment.Id,
                assessment.StudentId,
                assessment.AssessmentDate,
                assessment.Question1,
                assessment.Question2,
                assessment.Question3,
                assessment.Question4,
                assessment.Question5,
                assessment.Question6,
                assessment.Question7,
                assessment.IsCleared,
                assessment.Notes,
                assessment.CreatedAt);
    }
}
