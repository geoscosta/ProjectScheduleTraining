using MediatR;
using ProjectScheduleTraining.Application.ParQAssessments.DTOs;

namespace ProjectScheduleTraining.Application.ParQAssessments.Queries;

/// <summary>
/// Query responsável por retornar todas as avaliações PAR-Q de um aluno.
/// </summary>
public record GetAllParQAssessmentsByStudentIdQuery(Guid StudentId)
    : IRequest<IEnumerable<ParQAssessmentResponse>>;