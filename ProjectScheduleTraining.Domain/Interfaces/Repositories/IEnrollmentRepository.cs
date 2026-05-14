using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<Enrollment?> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Enrollment>> GetExpiringAsync(int days, CancellationToken cancellationToken = default);
    }
}
