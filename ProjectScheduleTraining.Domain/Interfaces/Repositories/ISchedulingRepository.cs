using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    public interface ISchedulingRepository : IRepository<Scheduling>
    {
        Task<IEnumerable<Scheduling>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Scheduling>> GetByScheduleIdAsync(Guid scheduleId, CancellationToken cancellationToken = default);
        Task<bool> SchedulingExistsAsync(Guid studentId, Guid scheduleId, CancellationToken cancellationToken = default);
    }
}
