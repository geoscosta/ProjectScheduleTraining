using MediatR;
using ProjectScheduleTraining.Application.StudentWorkouts.Commands;
using ProjectScheduleTraining.Application.StudentWorkouts.DTOs;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.StudentWorkouts.Handlers
{
    /// <summary>
    /// Handler responsável por atualizar um treino personalizado existente.
    /// Substitui todos os exercícios do treino pelos novos informados.
    /// </summary>
    public class UpdateStudentWorkoutHandler
        : IRequestHandler<UpdateStudentWorkoutCommand, StudentWorkoutResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStudentWorkoutHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de atualização do treino.
        /// Remove os exercícios anteriores e adiciona os novos.
        /// </summary>
        public async Task<StudentWorkoutResponse> Handle(
            UpdateStudentWorkoutCommand request,
            CancellationToken cancellationToken)
        {
            var workout = await _unitOfWork.StudentWorkouts
                .GetActiveByStudentIdAsync(request.Id, cancellationToken);

            if (workout is null)
                throw new DomainException(
                    "Treino não encontrado.",
                    "WORKOUT_NOT_FOUND");

            workout.Name = request.Name;
            workout.Description = request.Description;
            workout.EndDate = request.EndDate;
            workout.IsActive = request.IsActive;

            /// Remove os exercícios anteriores e adiciona os novos.
            workout.Exercises.Clear();
            foreach (var (exercise, index) in request.Exercises.Select((e, i) => (e, i)))
            {
                workout.Exercises.Add(new WorkoutExercise
                {
                    WorkoutId = workout.Id,
                    Name = exercise.Name,
                    MuscleGroup = exercise.MuscleGroup,
                    Sets = exercise.Sets,
                    Repetitions = exercise.Repetitions,
                    Load = exercise.Load,
                    RestSeconds = exercise.RestSeconds,
                    Notes = exercise.Notes,
                    Order = exercise.Order > 0 ? exercise.Order : index + 1,
                    VideoUrl = exercise.VideoUrl
                });
            }

            _unitOfWork.StudentWorkouts.Update(workout);
            await _unitOfWork.CommitAsync(cancellationToken);

            return MapToResponse(workout);
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
    }
}
