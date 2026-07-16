using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Retorna a matrícula ativa de um aluno, incluindo os dados do plano contratado.
        /// Retorna null caso o aluno não possua matrícula ativa.
        /// </summary>
        public async Task<Enrollment?> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Plan)
                .FirstOrDefaultAsync(x => x.StudentId == studentId && x.IsActive, cancellationToken);

        /// <summary>
        /// Retorna todas as matrículas ativas que vencem dentro do número de dias informado.
        /// Utilizado para envio de avisos de renovação.
        /// </summary>
        public async Task<IEnumerable<Enrollment>> GetExpiringAsync(int days, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.IsActive && x.ExpirationDate <= DateTime.UtcNow.AddDays(days))
                .ToListAsync(cancellationToken);
    }
}
