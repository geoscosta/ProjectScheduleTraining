namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        IPlanRepository Plans { get; }
        IEnrollmentRepository Enrollments { get; }
        IScheduleRepository Schedules { get; }
        ISchedulingRepository Schedulings { get; }
        IFinancialRepository Financials { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
