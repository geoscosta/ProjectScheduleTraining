using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Domain.Interfaces.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);
        Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        //Task<IEnumerable<Student>> GetActiveStudentsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Student>> GetOverdueStudentsAsync(CancellationToken cancellationToken = default);
        Task<bool> CpfExistsAsync(string cpf, Guid? excludeId = null, CancellationToken cancellationToken = default);
    }
}
