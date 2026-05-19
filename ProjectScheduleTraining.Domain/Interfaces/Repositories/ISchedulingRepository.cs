using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    public interface ISchedulingRepository : IRepository<Scheduling>
    {
        Task<IEnumerable<Scheduling>> GetByStudentIdAsync(Guid studentId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Scheduling>> GetByScheduleIdAsync(Guid scheduleId, CancellationToken cancellationToken = default);
        Task<bool> SchedulingExistsAsync(Guid studentId, Guid scheduleId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Busca um agendamento pelo identificador único incluindo
        /// os dados do aluno e do horário.
        /// </summary>
        Task<Scheduling?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
