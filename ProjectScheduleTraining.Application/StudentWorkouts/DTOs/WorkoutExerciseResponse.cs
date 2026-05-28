namespace ProjectScheduleTraining.Application.StudentWorkouts.DTOs
{
    /// <summary>
    /// DTO de resposta de exercício do treino.
    /// </summary>
    public record WorkoutExerciseResponse(
        Guid Id,
        string Name,
        string MuscleGroup,
        int Sets,
        string Repetitions,
        decimal? Load,
        int? RestSeconds,
        string? Notes,
        int Order,
        string? VideoUrl);

    /// <summary>
    /// DTO de resposta completa do treino personalizado.
    /// </summary>
    public record StudentWorkoutResponse(
        Guid Id,
        Guid StudentId,
        Guid TrainerId,
        string TrainerName,
        string Name,
        string? Description,
        DateTime StartDate,
        DateTime? EndDate,
        bool IsActive,
        DateTime CreatedAt,
        IEnumerable<WorkoutExerciseResponse> Exercises);

    /// <summary>
    /// DTO de resposta resumida do treino.
    /// </summary>
    public record StudentWorkoutSummaryResponse(
        Guid Id,
        string Name,
        string? Description,
        DateTime StartDate,
        bool IsActive,
        int ExerciseCount);
}
