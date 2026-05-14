using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    public class FinancialRepository : Repository<Financial>, IFinancialRepository
    {
        public FinancialRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Retorna todas as cobranças de um aluno, ordenadas da mais recente para a mais antiga.
        /// </summary>
        public async Task<IEnumerable<Financial>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.DueDate)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retorna todas as cobranças vencidas ou pendentes com data de vencimento ultrapassada.
        /// Utilizado para controle de inadimplência e bloqueio automático de alunos.
        /// </summary>
        public async Task<IEnumerable<Financial>> GetOverdueAsync(CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.Status == FinancialStatus.Overdue ||
                    (x.Status == FinancialStatus.Pending &&
                     x.DueDate < DateTime.UtcNow))
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retorna todas as cobranças pendentes que vencem dentro do número de dias informado.
        /// Utilizado para envio de lembretes de vencimento.
        /// </summary>
        public async Task<IEnumerable<Financial>> GetDueInDaysAsync(int days, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.Status == FinancialStatus.Pending &&
                    x.DueDate <= DateTime.UtcNow.AddDays(days))
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Calcula o total recebido em um determinado mês e ano.
        /// Utilizado para geração de relatórios financeiros mensais.
        /// </summary>
        public async Task<decimal> GetTotalReceivedByMonthAsync(int year, int month, CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.Status == FinancialStatus.Paid &&
                    x.PaymentDate!.Value.Year == year &&
                    x.PaymentDate!.Value.Month == month)
                .SumAsync(x => x.Amount, cancellationToken);
    }
}
