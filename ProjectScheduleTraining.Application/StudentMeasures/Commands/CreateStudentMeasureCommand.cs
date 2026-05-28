using MediatR;
using ProjectScheduleTraining.Application.StudentMeasures.DTOs;

namespace ProjectScheduleTraining.Application.StudentMeasures.Commands
{
    /// <summary>
    /// Command para registrar novas medidas corporais de um aluno.
    /// </summary>
    public record CreateStudentMeasureCommand(
        Guid StudentId,
        DateTime MeasureDate,
        decimal Weight,
        decimal Height,
        decimal? ChestCircumference,
        decimal? WaistCircumference,
        decimal? HipCircumference,
        decimal? ArmCircumference,
        decimal? ThighCircumference,
        decimal? CalfCircumference,
        decimal? BodyFatPercentage,
        decimal? LeanMassPercentage,
        string? Notes) : IRequest<StudentMeasureResponse>;
}
