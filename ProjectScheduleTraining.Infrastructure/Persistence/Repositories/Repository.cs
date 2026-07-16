using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Busca uma entidade pelo seu identificador único.
        /// Retorna null caso não seja encontrada.
        /// </summary>
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        /// <summary>
        /// Retorna todas as entidades não deletadas.
        /// O filtro de soft delete é aplicado automaticamente via QueryFilter global.
        /// </summary>
        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _dbSet.ToListAsync(cancellationToken);

        /// <summary>
        /// Adiciona uma nova entidade ao contexto.
        /// A persistência ocorre somente após o CommitAsync do UnitOfWork.
        /// </summary>
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await _dbSet.AddAsync(entity, cancellationToken);

        /// <summary>
        /// Marca a entidade como modificada no contexto.
        /// A persistência ocorre somente após o CommitAsync do UnitOfWork.
        /// </summary>
        public void Update(T entity)
            => _dbSet.Update(entity);

        /// <summary>
        /// Realiza o soft delete da entidade, marcando IsDeleted como true.
        /// O registro permanece no banco mas fica invisível via QueryFilter global.
        /// </summary>
        public void Delete(T entity)
            => entity.SoftDelete();
    }
}
