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
        IUserRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IStudentMeasureRepository StudentMeasures { get; }
        IParQAssessmentRepository ParQAssessments { get; }
        IStudentContractRepository StudentContracts { get; }
        IStudentWorkoutRepository StudentWorkouts { get; }
        IScheduleLockRepository ScheduleLocks { get; }

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
