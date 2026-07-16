using MediatR;
using ProjectScheduleTraining.Application.StudentWorkouts.DTOs;
using ProjectScheduleTraining.Application.StudentWorkouts.Queries;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.StudentWorkouts.Handlers
{
    /// <summary>
    /// Handler responsável por buscar treinos personalizados.
    /// </summary>
    public class GetStudentWorkoutHandler
        : IRequestHandler<GetActiveWorkoutByStudentIdQuery, StudentWorkoutResponse?>,
          IRequestHandler<GetAllWorkoutsByStudentIdQuery, IEnumerable<StudentWorkoutSummaryResponse>>,
          IRequestHandler<GetWorkoutsByTrainerIdQuery, IEnumerable<StudentWorkoutSummaryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStudentWorkoutHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retorna o treino ativo do aluno com todos os exercícios.
        /// </summary>
        public async Task<StudentWorkoutResponse?> Handle(
            GetActiveWorkoutByStudentIdQuery request,
            CancellationToken cancellationToken)
        {
            var workout = await _unitOfWork.StudentWorkouts
                .GetActiveByStudentIdAsync(request.StudentId, cancellationToken);

            return workout is null ? null : MapToResponse(workout);
        }

        /// <summary>
        /// Retorna todos os treinos do aluno em formato resumido.
        /// </summary>
        public async Task<IEnumerable<StudentWorkoutSummaryResponse>> Handle(
            GetAllWorkoutsByStudentIdQuery request,
            CancellationToken cancellationToken)
        {
            var workouts = await _unitOfWork.StudentWorkouts
                .GetAllByStudentIdAsync(request.StudentId, cancellationToken);

            return workouts.Select(MapToSummaryResponse);
        }

        /// <summary>
        /// Retorna todos os treinos criados por um professor.
        /// </summary>
        public async Task<IEnumerable<StudentWorkoutSummaryResponse>> Handle(
            GetWorkoutsByTrainerIdQuery request,
            CancellationToken cancellationToken)
        {
            var workouts = await _unitOfWork.StudentWorkouts
                .GetByTrainerIdAsync(request.TrainerId, cancellationToken);

            return workouts.Select(MapToSummaryResponse);
        }

        private static StudentWorkoutResponse MapToResponse(StudentWorkout workout)
            => new(
                workout.Id,
                workout.StudentId,
                workout.TrainerId,
                workout.Trainer?.Name ?? string.Empty,
                workout.Name,
                workout.Description,
                workout.StartDate,
                workout.EndDate,
                workout.IsActive,
                workout.CreatedAt,
                workout.Exercises.Select(e => new WorkoutExerciseResponse(
                    e.Id,
                    e.Name,
                    e.MuscleGroup,
                    e.Sets,
                    e.Repetitions,
                    e.Load,
                    e.RestSeconds,
                    e.Notes,
                    e.Order,
                    e.VideoUrl)));

        private static StudentWorkoutSummaryResponse MapToSummaryResponse(StudentWorkout workout)
            => new(
                workout.Id,
                workout.Name,
                workout.Description,
                workout.StartDate,
                workout.IsActive,
                workout.Exercises.Count);
    }
}
