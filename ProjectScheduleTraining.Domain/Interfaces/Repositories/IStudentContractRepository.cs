using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface do repositório de contratos digitais dos alunos.
    /// </summary>
    public interface IStudentContractRepository : IRepository<StudentContract>
    {
        /// <summary>
        /// Retorna o contrato ativo vinculado a uma matrícula.
        /// </summary>
        Task<StudentContract?> GetByEnrollmentIdAsync(
            Guid enrollmentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna todos os contratos de um aluno.
        /// </summary>
        Task<IEnumerable<StudentContract>> GetByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica se a matrícula já possui contrato assinado.
        /// </summary>
        Task<bool> HasSignedContractAsync(
            Guid enrollmentId,
            CancellationToken cancellationToken = default);
    }
}
