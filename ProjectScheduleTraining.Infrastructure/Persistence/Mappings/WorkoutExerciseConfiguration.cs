using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    /// <summary>
    /// Configuração EF Core da entidade WorkoutExercise.
    /// Define mapeamento de tabela, colunas e relacionamentos.
    /// </summary>
    public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
    {
        public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
        {
            builder.ToTable("WorkoutExercises");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.WorkoutId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.MuscleGroup)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Sets)
                .IsRequired();

            builder.Property(x => x.Repetitions)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Load)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.RestSeconds);

            builder.Property(x => x.Notes)
                .HasMaxLength(300);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.VideoUrl)
                .HasMaxLength(500);

            /// Relacionamento com StudentWorkout.
            builder.HasOne(x => x.Workout)
                .WithMany(x => x.Exercises)
                .HasForeignKey(x => x.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
