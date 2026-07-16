using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface do repositório de avaliações PAR-Q.
    /// </summary>
    public interface IParQAssessmentRepository : IRepository<ParQAssessment>
    {
        /// <summary>
        /// Retorna a avaliação PAR-Q mais recente de um aluno.
        /// </summary>
        Task<ParQAssessment?> GetLatestByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica se o aluno possui avaliação PAR-Q válida.
        /// </summary>
        Task<bool> HasValidAssessmentAsync(
            Guid studentId,
            CancellationToken cancellationToken = default);
    }
}
