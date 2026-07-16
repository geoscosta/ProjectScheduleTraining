using Microsoft.EntityFrameworkCore;
using ProjectScheduleTraining.Domain.Entities;
using ProjectScheduleTraining.Domain.Interfaces.Repositories;
using ProjectScheduleTraining.Infrastructure.Persistence.Context;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Repositório responsável pelas operações de treinos personalizados.
    /// </summary>
    public class StudentWorkoutRepository : Repository<StudentWorkout>, IStudentWorkoutRepository
    {
        public StudentWorkoutRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Retorna o treino ativo de um aluno com todos os exercícios incluídos.
        /// Exercícios são ordenados pela propriedade Order para exibição correta.
        /// </summary>
        public async Task<StudentWorkout?> GetActiveByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Exercises.OrderBy(e => e.Order))
                .Include(x => x.Trainer)
                .FirstOrDefaultAsync(
                    x => x.StudentId == studentId && x.IsActive,
                    cancellationToken);

        /// <summary>
        /// Retorna todos os treinos de um aluno ordenados do mais recente.
        /// Inclui exercícios para exibição no histórico de treinos.
        /// </summary>
        public async Task<IEnumerable<StudentWorkout>> GetAllByStudentIdAsync(
            Guid studentId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Exercises.OrderBy(e => e.Order))
                .Include(x => x.Trainer)
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync(cancellationToken);

        /// <summary>
        /// Retorna todos os treinos criados por um professor específico.
        /// Utilizado pelo professor para gerenciar seus alunos.
        /// </summary>
        public async Task<IEnumerable<StudentWorkout>> GetByTrainerIdAsync(
            Guid trainerId,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Include(x => x.Student)
                .Include(x => x.Exercises.OrderBy(e => e.Order))
                .Where(x => x.TrainerId == trainerId)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync(cancellationToken);
    }
}
