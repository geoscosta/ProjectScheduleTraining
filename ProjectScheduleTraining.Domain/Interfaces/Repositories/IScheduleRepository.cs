using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<IEnumerable<Schedule>> GetByPeriodAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default);
        Task<IEnumerable<Schedule>> GetAvailableAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<Schedule?> GetWithSchedulingsAsync(Guid scheduleId, CancellationToken cancellationToken = default);
    }
}
