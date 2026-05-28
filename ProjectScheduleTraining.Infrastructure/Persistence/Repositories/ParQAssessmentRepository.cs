using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repositório responsável pelas operações de avaliações PAR-Q.
    /// </summary>
    public class ParQAssessmentRepository : Repository<ParQAssessment>, IParQAssessmentRepository
    {
        public ParQAssessmentRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Retorna a avaliação PAR-Q mais recente de um aluno.
        /// </summary>
        public async Task<ParQAssessment?> GetLatestByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.AssessmentDate)
                .FirstOrDefaultAsync(cancellationToken);

        /// <summary>
        /// Verifica se o aluno possui avaliação PAR-Q válida.
        /// Uma avaliação é válida se foi preenchida e o aluno foi liberado (IsCleared = true).
        /// </summary>
        public async Task<bool> HasValidAssessmentAsync(
            Guid studentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .AnyAsync(x => x.StudentId == studentId && x.IsCleared, cancellationToken);
    }
}
