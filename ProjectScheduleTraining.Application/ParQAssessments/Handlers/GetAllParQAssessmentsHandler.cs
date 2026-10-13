using MediatR;
using ProjectScheduleTraining.Application.ParQAssessments.DTOs;
using ProjectScheduleTraining.Application.ParQAssessments.Queries;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.ParQAssessments.Handlers;

/// <summary>
/// Handler responsável por retornar todas as avaliações PAR-Q de um aluno.
/// </summary>
public class GetAllParQAssessmentsHandler
    : IRequestHandler<GetAllParQAssessmentsByStudentIdQuery, IEnumerable<ParQAssessmentResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllParQAssessmentsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Retorna todas as avaliações PAR-Q do aluno ordenadas da mais recente.
    /// </summary>
    public async Task<IEnumerable<ParQAssessmentResponse>> Handle(
        GetAllParQAssessmentsByStudentIdQuery request,
        CancellationToken cancellationToken)
    {
        var assessments = await _unitOfWork.ParQAssessments
            .GetAllByStudentIdAsync(request.StudentId, cancellationToken);

        return assessments.Select(MapToResponse);
    }

    private static ParQAssessmentResponse MapToResponse(ParQAssessment a)
        => new(
            a.Id,
            a.StudentId,
            a.AssessmentDate,
            a.Question1,
            a.Question2,
            a.Question3,
            a.Question4,
            a.Question5,
            a.Question6,
            a.Question7,
            a.IsCleared,
            a.Notes,
            a.CreatedAt);
}