using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    public class SchedulingRepository : Repository<Scheduling>, ISchedulingRepository
    {
        public SchedulingRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Retorna todos os agendamentos de um aluno, incluindo os dados do horário.
        /// Ordenado do mais recente para o mais antigo.
        /// </summary>
        public async Task<IEnumerable<Scheduling>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Schedule)
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.Schedule!.Date)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retorna todos os agendamentos de um horário específico, incluindo os dados dos alunos.
        /// Utilizado para controle de presença em uma turma.
        /// </summary>
        public async Task<IEnumerable<Scheduling>> GetByScheduleIdAsync(Guid scheduleId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Student)
                .Where(x => x.ScheduleId == scheduleId)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Verifica se já existe um agendamento para o aluno no horário informado.
        /// Utilizado para evitar agendamentos duplicados.
        /// </summary>
        public async Task<bool> SchedulingExistsAsync(Guid studentId, Guid scheduleId, CancellationToken cancellationToken = default)
            => await _dbSet.AnyAsync(
                x => x.StudentId == studentId && x.ScheduleId == scheduleId,
                cancellationToken);
    }
}
