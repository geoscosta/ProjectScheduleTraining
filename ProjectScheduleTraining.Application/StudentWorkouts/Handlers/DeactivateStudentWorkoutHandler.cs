using MediatR;
using ProjectScheduleTraining.Application.StudentWorkouts.Commands;
using ProjectScheduleTraining.Domain.Exceptions;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;

namespace ProjectScheduleTraining.Application.StudentWorkouts.Handlers
{
    /// <summary>
    /// Handler responsável por desativar um treino personalizado.
    /// </summary>
    public class DeactivateStudentWorkoutHandler
        : IRequestHandler<DeactivateStudentWorkoutCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateStudentWorkoutHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Processa o comando de desativação do treino.
        /// Lança exceção caso o treino não seja encontrado.
        /// </summary>
        public async Task Handle(
            DeactivateStudentWorkoutCommand request,
            CancellationToken cancellationToken)
        {
            var workout = await _unitOfWork.StudentWorkouts
                .GetByIdAsync(request.Id, cancellationToken);

            if (workout is null)
                throw new DomainException(
                    "Treino não encontrado.",
                    "WORKOUT_NOT_FOUND");

            if (!workout.IsActive)
                throw new DomainException(
                    "Treino já está desativado.",
                    "WORKOUT_ALREADY_INACTIVE");

            workout.IsActive = false;

            _unitOfWork.StudentWorkouts.Update(workout);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
