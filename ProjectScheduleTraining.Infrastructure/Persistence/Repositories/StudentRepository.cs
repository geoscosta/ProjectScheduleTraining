using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Enums;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Busca um aluno pelo CPF.
        /// Retorna null caso não seja encontrado.
        /// </summary>
        public async Task<Student?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.Cpf == cpf, cancellationToken);

        /// <summary>
        /// Busca um aluno pelo e-mail.
        /// A comparação é feita em lowercase para evitar duplicatas por diferença de caixa.
        /// </summary>
        public async Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.Email == email.ToLower(), cancellationToken);

        /// <summary>
        /// Retorna todos os alunos com status ativo, ordenados por nome.
        /// </summary>
        public async Task<IEnumerable<Student>> GetActiveStudentsAsync(CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.Status == StudentStatus.Active)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retorna todos os alunos que possuem cobranças vencidas ou pendentes com data de vencimento ultrapassada.
        /// Utilizado para controle de inadimplência e bloqueio automático.
        /// </summary>
        public async Task<IEnumerable<Student>> GetOverdueStudentsAsync(CancellationToken cancellationToken = default)
            => await _dbSet
                .Where(x => x.Financials!.Any(f =>
                    f.Status == FinancialStatus.Overdue ||
                    (f.Status == FinancialStatus.Pending &&
                     f.DueDate < DateTime.UtcNow)))
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Verifica se já existe um aluno cadastrado com o CPF informado.
        /// O parâmetro excludeId permite ignorar o próprio aluno em operações de atualização.
        /// </summary>
        public async Task<bool> CpfExistsAsync(string cpf, Guid? excludeId = null, CancellationToken cancellationToken = default)
            => await _dbSet.AnyAsync(
                x => x.Cpf == cpf && (excludeId == null || x.Id != excludeId),
                cancellationToken);
    }
}
