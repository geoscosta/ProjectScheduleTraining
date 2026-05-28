using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repositório responsável pelas operações de medidas corporais dos alunos.
    /// </summary>
    public class StudentMeasureRepository : Repository<StudentMeasure>, IStudentMeasureRepository
    {
        public StudentMeasureRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Retorna todas as medidas de um aluno ordenadas por data decrescente.
        /// </summary>
        public async Task<IEnumerable<StudentMeasure>> GetByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.MeasureDate)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retorna a medida mais recente de um aluno.
        /// Utilizado para exibir o último registro na tela de detalhes.
        /// </summary>
        public async Task<StudentMeasure?> GetLatestByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.MeasureDate)
                .FirstOrDefaultAsync(cancellationToken);
    }
}
