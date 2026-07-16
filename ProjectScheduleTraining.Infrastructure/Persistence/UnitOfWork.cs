using Microsoft.EntityFrameworkCore.Storage;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;
using ProjectScheduleTraining.Infrastructure.Persistence.Repositories;

namespace ProjectScheduleTraining.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public IStudentRepository Students { get; }
    public IPlanRepository Plans { get; }
    public IEnrollmentRepository Enrollments { get; }
    public IScheduleRepository Schedules { get; }
    public ISchedulingRepository Schedulings { get; }
    public IFinancialRepository Financials { get; }
    public IUserRepository Users { get; }
    public IRefreshTokenRepository RefreshTokens { get; }
    public IStudentMeasureRepository StudentMeasures { get; }
    public IParQAssessmentRepository ParQAssessments { get; }
    public IStudentContractRepository StudentContracts { get; }
    public IStudentWorkoutRepository StudentWorkouts { get; }
    public IScheduleLockRepository ScheduleLocks { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Students = new StudentRepository(context);
        Plans = new PlanRepository(context);
        Enrollments = new EnrollmentRepository(context);
        Schedules = new ScheduleRepository(context);
        Schedulings = new SchedulingRepository(context);
        Financials = new FinancialRepository(context);
        Users = new UserRepository(context);
        RefreshTokens = new RefreshTokenRepository(context);
        StudentMeasures = new StudentMeasureRepository(context);
        ParQAssessments = new ParQAssessmentRepository(context);
        StudentContracts = new StudentContractRepository(context);
        StudentWorkouts = new StudentWorkoutRepository(context);
        ScheduleLocks = new ScheduleLockRepository(context);
    }

    /// <summary>
    /// Persiste todas as alterações pendentes no banco de dados.
    /// </summary>
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    /// <summary>
    /// Inicia uma transação no banco de dados.
    /// </summary>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    /// <summary>
    /// Confirma a transação atual.
    /// </summary>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            await _transaction.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Reverte a transação atual em caso de erro.
    /// </summary>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            await _transaction.RollbackAsync(cancellationToken);
    }

    /// <summary>
    /// Libera apenas a transação. O AppDbContext é gerenciado pelo container DI (Scoped)
    /// e será disposto automaticamente ao final do request — não chamar Dispose() aqui
    /// evita ObjectDisposedException caso o contexto seja acessado após este Dispose
    /// dentro do mesmo scope de DI.
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        // Nota intencional: _context.Dispose() NÃO é chamado aqui.
        // O AppDbContext tem lifetime Scoped e é gerenciado pelo container ASP.NET Core,
        // que o descarta corretamente ao final do pipeline HTTP.
    }
}
