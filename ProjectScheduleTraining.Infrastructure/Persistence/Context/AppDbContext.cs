using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectScheduleTraining.Domain.Common;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Scheduling> Schedulings => Set<Scheduling>();
    public DbSet<Financial> Financials => Set<Financial>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<StudentMeasure> StudentMeasures => Set<StudentMeasure>();
    public DbSet<ParQAssessment> ParQAssessments => Set<ParQAssessment>();
    public DbSet<StudentContract> StudentContracts => Set<StudentContract>();
    public DbSet<StudentWorkout> StudentWorkouts => Set<StudentWorkout>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<ScheduleLock> ScheduleLocks => Set<ScheduleLock>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Registra automaticamente todas as IEntityTypeConfiguration do assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Soft delete — filtro global aplicado a TODAS as 14 entidades.
        // Garante que registros marcados como IsDeleted = true nunca apareçam
        // em nenhuma query, a menos que IgnoreQueryFilters() seja explicitamente chamado.
        modelBuilder.Entity<Student>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Plan>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Enrollment>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Schedule>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Scheduling>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Financial>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<RefreshToken>().HasQueryFilter(x => !x.IsDeleted);

        // As 6 entidades abaixo estavam sem HasQueryFilter — adicionadas neste fix.
        // Sem o filtro, dados soft-deletados apareciam em todas as queries dessas entidades.
        modelBuilder.Entity<StudentMeasure>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ParQAssessment>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<StudentContract>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<StudentWorkout>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<WorkoutExercise>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<ScheduleLock>().HasQueryFilter(x => !x.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AuditEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Atualiza automaticamente UpdatedAt em todas as entidades modificadas
    /// e protege CreatedAt contra modificações acidentais.
    /// </summary>
    private void AuditEntities()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(BaseEntity.UpdatedAt)).CurrentValue = DateTime.UtcNow;
                entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
            }
        }
    }
}
