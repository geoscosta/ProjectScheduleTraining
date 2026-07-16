using MediatR;
using ProjectScheduleTraining.Application.StudentMeasures.Commands;
using ProjectScheduleTraining.Application.StudentMeasures.DTOs;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.StudentMeasures.Handlers
{
    /// <summary>
    /// Handler responsável por registrar novas medidas corporais de um aluno.
    /// Calcula o IMC automaticamente a partir do peso e altura informados.
    /// </summary>
    public class CreateStudentMeasureHandler
        : IRequestHandler<CreateStudentMeasureCommand, StudentMeasureResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudentMeasureHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de registro de medidas corporais.
        /// Valida a existência do aluno e calcula o IMC automaticamente.
        /// </summary>
        public async Task<StudentMeasureResponse> Handle(
            CreateStudentMeasureCommand request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            /// Calcula o IMC: peso (kg) / altura² (m).
            var heightInMeters = request.Height / 100;
            var bmi = Math.Round(request.Weight / (heightInMeters * heightInMeters), 2);

            var measure = new StudentMeasure
            {
                StudentId = request.StudentId,
                MeasureDate = request.MeasureDate,
                Weight = request.Weight,
                Height = request.Height,
                Bmi = bmi,
                ChestCircumference = request.ChestCircumference,
                WaistCircumference = request.WaistCircumference,
                HipCircumference = request.HipCircumference,
                ArmCircumference = request.ArmCircumference,
                ThighCircumference = request.ThighCircumference,
                CalfCircumference = request.CalfCircumference,
                BodyFatPercentage = request.BodyFatPercentage,
                LeanMassPercentage = request.LeanMassPercentage,
                Notes = request.Notes
            };

            await _unitOfWork.StudentMeasures.AddAsync(measure, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return MapToResponse(measure);
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
