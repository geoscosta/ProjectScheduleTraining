using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repositório responsável pelas operações de trancamentos de agenda.
    /// </summary>
    public class ScheduleLockRepository : Repository<ScheduleLock>, IScheduleLockRepository
    {
        public ScheduleLockRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Retorna todos os trancamentos de um aluno ordenados por data de início.
        /// </summary>
        public async Task<IEnumerable<ScheduleLock>> GetByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.LockStartDate)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Verifica se o aluno já utilizou o trancamento no semestre informado.
        /// Cada aluno fidelidade tem direito a 1 trancamento aprovado por semestre.
        /// </summary>
        public async Task<bool> HasLockInSemesterAsync(
            Guid studentId,
            Semester semester,
            int year,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .AnyAsync(
                    x => x.StudentId == studentId &&
                         x.Semester == semester &&
                         x.Year == year &&
                         x.IsApproved,
                    cancellationToken);

        /// <summary>
        /// Retorna o trancamento ativo de um aluno se existir.
        /// Um trancamento está ativo quando a data atual está dentro do período.
        /// </summary>
        public async Task<ScheduleLock?> GetActiveLockByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.StudentId == studentId &&
                            x.IsApproved &&
                            x.LockStartDate <= DateTime.UtcNow &&
                            x.LockEndDate >= DateTime.UtcNow)
                .FirstOrDefaultAsync(cancellationToken);
    }
}
