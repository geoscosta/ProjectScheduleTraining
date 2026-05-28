using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface do repositório de treinos personalizados.
    /// </summary>
    public interface IStudentWorkoutRepository : IRepository<StudentWorkout>
    {
        /// <summary>
        /// Retorna o treino ativo de um aluno com os exercícios incluídos.
        /// </summary>
        Task<StudentWorkout?> GetActiveByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna todos os treinos de um aluno.
        /// </summary>
        Task<IEnumerable<StudentWorkout>> GetAllByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna todos os treinos criados por um professor.
        /// </summary>
        Task<IEnumerable<StudentWorkout>> GetByTrainerIdAsync(
            Guid trainerId,
            CancellationToken cancellationToken = default);
    }
}
