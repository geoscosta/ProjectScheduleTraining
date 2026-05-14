using Microsoft.EntityFrameworkCore.Storage;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;
using ProjectScheduleTraining.Infrastructure.Persistence.Repositories;

namespace ProjectScheduleTraining.Infrastructure.Persistence
{
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

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Students = new StudentRepository(context);
            Plans = new PlanRepository(context);
            Enrollments = new EnrollmentRepository(context);
            Schedules = new ScheduleRepository(context);
            Schedulings = new SchedulingRepository(context);
            Financials = new FinancialRepository(context);
        }

        /// <summary>
        /// Persiste todas as alterações pendentes no banco de dados.
        /// Deve ser chamado ao final de cada operação de escrita.
        /// </summary>
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken);

        /// <summary>
        /// Inicia uma transação no banco de dados.
        /// Utilizado quando múltiplas operações precisam ser executadas de forma atômica.
        /// </summary>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
            => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        /// <summary>
        /// Confirma a transação atual, tornando todas as operações permanentes no banco.
        /// </summary>
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
                await _transaction.CommitAsync(cancellationToken);
        }

        /// <summary>
        /// Reverte a transação atual, desfazendo todas as operações realizadas desde o BeginTransactionAsync.
        /// Chamado automaticamente em caso de erro no pipeline de transação.
        /// </summary>
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
                await _transaction.RollbackAsync(cancellationToken);
        }

        /// <summary>
        /// Libera os recursos da transação e do contexto do banco de dados.
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
