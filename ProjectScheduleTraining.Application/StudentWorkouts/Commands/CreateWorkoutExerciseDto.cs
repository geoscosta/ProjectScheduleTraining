using MediatR;
using ProjectScheduleTraining.Application.StudentWorkouts.DTOs;

namespace ProjectScheduleTraining.Application.StudentWorkouts.Commands
{
    /// <summary>
    /// DTO de criação de exercício.
    /// </summary>
    public record CreateWorkoutExerciseDto(
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
    /// Command para criar um treino personalizado para um aluno.
    /// </summary>
    public record CreateStudentWorkoutCommand(
        Guid StudentId,
        Guid TrainerId,
        string Name,
        string? Description,
        DateTime StartDate,
        DateTime? EndDate,
        IEnumerable<CreateWorkoutExerciseDto> Exercises)
        : IRequest<StudentWorkoutResponse>;

    /// <summary>
    /// Command para atualizar um treino existente.
    /// </summary>
    public record UpdateStudentWorkoutCommand(
        Guid Id,
        string Name,
        string? Description,
        DateTime? EndDate,
        bool IsActive,
        IEnumerable<CreateWorkoutExerciseDto> Exercises)
        : IRequest<StudentWorkoutResponse>;

    /// <summary>
    /// Command para desativar um treino.
    /// </summary>
    public record DeactivateStudentWorkoutCommand(Guid Id) : IRequest;
}
