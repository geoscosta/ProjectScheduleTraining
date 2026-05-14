using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Retorna todos os horários de uma data específica, ordenados por hora de início.
        /// </summary>
        public async Task<IEnumerable<Schedule>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.Date == date.Date)
                .OrderBy(x => x.StartTime)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retorna todos os horários dentro de um período, ordenados por data e hora de início.
        /// Utilizado para visualização semanal e mensal da agenda.
        /// </summary>
        public async Task<IEnumerable<Schedule>> GetByPeriodAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.Date >= start.Date && x.Date <= end.Date)
                .OrderBy(x => x.Date)
                .ThenBy(x => x.StartTime)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retorna apenas os horários disponíveis de uma data,
        /// ou seja, com vagas e sem bloqueio.
        /// </summary>
        public async Task<IEnumerable<Schedule>> GetAvailableAsync(DateTime date, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.Date == date.Date && x.Status == ScheduleStatus.Available)
                .OrderBy(x => x.StartTime)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retorna um horário com todos os agendamentos e dados dos alunos incluídos.
        /// Utilizado para visualização detalhada de uma turma.
        /// </summary>
        public async Task<Schedule?> GetWithSchedulingsAsync(Guid scheduleId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Schedulings)
                .ThenInclude(x => x.Student)
                .FirstOrDefaultAsync(x => x.Id == scheduleId, cancellationToken);
    }
}
