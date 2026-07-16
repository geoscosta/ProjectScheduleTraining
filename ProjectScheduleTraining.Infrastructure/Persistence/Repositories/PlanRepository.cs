using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        public PlanRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Retorna todos os planos ativos disponíveis para matrícula, ordenados por nome.
        /// </summary>
        public async Task<IEnumerable<Plan>> GetActivePlansAsync(CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
    }
}
