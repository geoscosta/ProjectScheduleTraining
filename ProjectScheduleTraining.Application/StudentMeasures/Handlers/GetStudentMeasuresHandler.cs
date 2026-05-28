using MediatR;
using ProjectScheduleTraining.Application.StudentMeasures.DTOs;
using ProjectScheduleTraining.Application.StudentMeasures.Queries;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.StudentMeasures.Handlers
{
    /// <summary>
    /// Handler responsável por buscar as medidas corporais de um aluno.
    /// </summary>
    public class GetStudentMeasuresHandler
        : IRequestHandler<GetStudentMeasuresByStudentIdQuery, IEnumerable<StudentMeasureResponse>>,
          IRequestHandler<GetLatestStudentMeasureQuery, StudentMeasureResponse?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStudentMeasuresHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retorna todas as medidas de um aluno ordenadas por data.
        /// </summary>
        public async Task<IEnumerable<StudentMeasureResponse>> Handle(
            GetStudentMeasuresByStudentIdQuery request,
            CancellationToken cancellationToken)
        {
            var measures = await _unitOfWork.StudentMeasures
                .GetByStudentIdAsync(request.StudentId, cancellationToken);

            return measures.Select(MapToResponse);
        }

        /// <summary>
        /// Retorna a medida mais recente de um aluno.
        /// </summary>
        public async Task<StudentMeasureResponse?> Handle(
            GetLatestStudentMeasureQuery request,
            CancellationToken cancellationToken)
        {
            var measure = await _unitOfWork.StudentMeasures
                .GetLatestByStudentIdAsync(request.StudentId, cancellationToken);

            return measure is null ? null : MapToResponse(measure);
        }

        /// <summary>
        /// Mapeia a entidade StudentMeasure para o DTO de resposta.
        /// </summary>
        private static StudentMeasureResponse MapToResponse(StudentMeasure measure)
            => new(
                measure.Id,
                measure.StudentId,
                measure.MeasureDate,
                measure.Weight,
                measure.Height,
                measure.Bmi,
                measure.ChestCircumference,
                measure.WaistCircumference,
                measure.HipCircumference,
                measure.ArmCircumference,
                measure.ThighCircumference,
                measure.CalfCircumference,
                measure.BodyFatPercentage,
                measure.LeanMassPercentage,
                measure.Notes,
                measure.CreatedAt);
    }
}
