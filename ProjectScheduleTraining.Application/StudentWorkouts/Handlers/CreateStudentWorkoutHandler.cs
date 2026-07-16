using MediatR;
using ProjectScheduleTraining.Application.StudentWorkouts.Commands;
using ProjectScheduleTraining.Application.StudentWorkouts.DTOs;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.StudentWorkouts.Handlers
{
    /// <summary>
    /// Handler responsável por criar treinos personalizados para alunos.
    /// Apenas professores e administradores podem criar treinos.
    /// </summary>
    public class CreateStudentWorkoutHandler
        : IRequestHandler<CreateStudentWorkoutCommand, StudentWorkoutResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudentWorkoutHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de criação de treino personalizado.
        /// Desativa o treino anterior ativo antes de criar o novo.
        /// </summary>
        public async Task<StudentWorkoutResponse> Handle(
            CreateStudentWorkoutCommand request,
            CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.Students
                .GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null)
                throw new DomainException(
                    "Aluno não encontrado.",
                    "STUDENT_NOT_FOUND");

            var trainer = await _unitOfWork.Users
                .GetByIdAsync(request.TrainerId, cancellationToken);

            if (trainer is null)
                throw new DomainException(
                    "Professor não encontrado.",
                    "TRAINER_NOT_FOUND");

            /// Desativa o treino ativo anterior antes de criar o novo.
            var activeWorkout = await _unitOfWork.StudentWorkouts
                .GetActiveByStudentIdAsync(request.StudentId, cancellationToken);

            if (activeWorkout is not null)
            {
                activeWorkout.IsActive = false;
                _unitOfWork.StudentWorkouts.Update(activeWorkout);
            }

            var workout = new StudentWorkout
            {
                StudentId = request.StudentId,
                TrainerId = request.TrainerId,
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true,
                Exercises = request.Exercises.Select((e, index) => new WorkoutExercise
                {
                    Name = e.Name,
                    MuscleGroup = e.MuscleGroup,
                    Sets = e.Sets,
                    Repetitions = e.Repetitions,
                    Load = e.Load,
                    RestSeconds = e.RestSeconds,
                    Notes = e.Notes,
                    Order = e.Order > 0 ? e.Order : index + 1,
                    VideoUrl = e.VideoUrl
                }).ToList()
            };

            await _unitOfWork.StudentWorkouts.AddAsync(workout, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            workout.Trainer = trainer;

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
