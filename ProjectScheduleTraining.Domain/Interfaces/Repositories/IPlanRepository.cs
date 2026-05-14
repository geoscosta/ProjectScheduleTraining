using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    public interface IPlanRepository : IRepository<Plan>
    {
        Task<IEnumerable<Plan>> GetActivePlansAsync(CancellationToken cancellationToken = default);
    }
}
