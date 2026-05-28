using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface do repositório de trancamentos de agenda.
    /// </summary>
    public interface IScheduleLockRepository : IRepository<ScheduleLock>
    {
        /// <summary>
        /// Retorna todos os trancamentos de um aluno.
        /// </summary>
        Task<IEnumerable<ScheduleLock>> GetByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica se o aluno já utilizou o trancamento no semestre informado.
        /// Cada aluno tem direito a apenas 1 trancamento por semestre.
        /// </summary>
        Task<bool> HasLockInSemesterAsync(
            Guid studentId,
            Semester semester,
            int year,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna o trancamento ativo de um aluno se existir.
        /// </summary>
        Task<ScheduleLock?> GetActiveLockByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default);
    }
}
