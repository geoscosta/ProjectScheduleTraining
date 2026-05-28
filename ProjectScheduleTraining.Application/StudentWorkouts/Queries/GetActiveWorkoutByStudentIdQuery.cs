using MediatR;
using ProjectScheduleTraining.Application.StudentWorkouts.DTOs;

namespace ProjectScheduleTraining.Application.StudentWorkouts.Queries
{
    /// <summary>
    /// Query para buscar o treino ativo de um aluno.
    /// </summary>
    public record GetActiveWorkoutByStudentIdQuery(Guid StudentId)
        : IRequest<StudentWorkoutResponse?>;

    /// <summary>
    /// Query para listar todos os treinos de um aluno.
    /// </summary>
    public record GetAllWorkoutsByStudentIdQuery(Guid StudentId)
        : IRequest<IEnumerable<StudentWorkoutSummaryResponse>>;

    /// <summary>
    /// Query para listar todos os treinos criados por um professor.
    /// </summary>
    public record GetWorkoutsByTrainerIdQuery(Guid TrainerId)
        : IRequest<IEnumerable<StudentWorkoutSummaryResponse>>;
}
