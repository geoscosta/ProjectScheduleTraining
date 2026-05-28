using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repositório responsável pelas operações de contratos digitais dos alunos.
    /// </summary>
    public class StudentContractRepository : Repository<StudentContract>, IStudentContractRepository
    {
        public StudentContractRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Retorna o contrato vinculado a uma matrícula específica.
        /// </summary>
        public async Task<StudentContract?> GetByEnrollmentIdAsync(
            Guid enrollmentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId, cancellationToken);

        /// <summary>
        /// Retorna todos os contratos de um aluno ordenados por data de assinatura.
        /// </summary>
        public async Task<IEnumerable<StudentContract>> GetByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.SignedAt)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Verifica se a matrícula já possui contrato assinado.
        /// Utilizado para impedir duplicidade de contratos por matrícula.
        /// </summary>
        public async Task<bool> HasSignedContractAsync(
            Guid enrollmentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .AnyAsync(x => x.EnrollmentId == enrollmentId && x.IsSigned, cancellationToken);
    }
}
