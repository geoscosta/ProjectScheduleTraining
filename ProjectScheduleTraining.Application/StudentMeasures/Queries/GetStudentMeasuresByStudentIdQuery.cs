using MediatR;
using ProjectScheduleTraining.Application.StudentMeasures.DTOs;

namespace ProjectScheduleTraining.Application.StudentMeasures.Queries
{
    /// <summary>
    /// Query para listar todas as medidas de um aluno.
    /// </summary>
    public record GetStudentMeasuresByStudentIdQuery(Guid StudentId)
        : IRequest<IEnumerable<StudentMeasureResponse>>;

    /// <summary>
    /// Query para buscar a medida mais recente de um aluno.
    /// </summary>
    public record GetLatestStudentMeasureQuery(Guid StudentId)
        : IRequest<StudentMeasureResponse?>;
}
