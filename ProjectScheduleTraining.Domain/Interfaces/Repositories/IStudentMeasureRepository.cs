using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface do repositório de medidas corporais do aluno.
    /// </summary>
    public interface IStudentMeasureRepository : IRepository<StudentMeasure>
    {
        /// <summary>
        /// Retorna todas as medidas de um aluno ordenadas por data.
        /// </summary>
        Task<IEnumerable<StudentMeasure>> GetByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna a medida mais recente de um aluno.
        /// </summary>
        Task<StudentMeasure?> GetLatestByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default);
    }
}
