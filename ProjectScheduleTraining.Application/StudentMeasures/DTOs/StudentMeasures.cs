namespace ProjectScheduleTraining.Application.StudentMeasures.DTOs
{
    /// <summary>
    /// DTO de resposta completa de medidas corporais.
    /// </summary>
    public record StudentMeasureResponse(
        Guid Id,
        Guid StudentId,
        DateTime MeasureDate,
        decimal Weight,
        decimal Height,
        decimal Bmi,
        decimal? ChestCircumference,
        decimal? WaistCircumference,
        decimal? HipCircumference,
        decimal? ArmCircumference,
        decimal? ThighCircumference,
        decimal? CalfCircumference,
        decimal? BodyFatPercentage,
        decimal? LeanMassPercentage,
        string? Notes,
        DateTime CreatedAt);
}
