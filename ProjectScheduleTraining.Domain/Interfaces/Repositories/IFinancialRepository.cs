using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    public interface IFinancialRepository : IRepository<Financial>
    {
        Task<IEnumerable<Financial>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Financial>> GetOverdueAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Financial>> GetDueInDaysAsync(int days, CancellationToken cancellationToken = default);
        Task<decimal> GetTotalReceivedByMonthAsync(int year, int month, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retorna os IDs dos alunos com cobranças vencidas
        /// há mais de N dias e que ainda estão ativos.
        /// </summary>
        Task<IEnumerable<Guid>> GetStudentIdsWithOverdueFinancialsAsync(int daysOverdue, CancellationToken cancellationToken = default);
    }
}
