using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    /// <summary>
    /// Configuração EF Core da entidade StudentWorkout.
    /// Define mapeamento de tabela, colunas e relacionamentos.
    /// </summary>
    public class StudentWorkoutConfiguration : IEntityTypeConfiguration<StudentWorkout>
    {
        public void Configure(EntityTypeBuilder<StudentWorkout> builder)
        {
            builder.ToTable("StudentWorkouts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.TrainerId)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired();

            /// Relacionamento com Student.
            builder.HasOne(x => x.Student)
                .WithMany(x => x.Workouts)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            /// Relacionamento com Trainer (User).
            builder.HasOne(x => x.Trainer)
                .WithMany()
                .HasForeignKey(x => x.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
