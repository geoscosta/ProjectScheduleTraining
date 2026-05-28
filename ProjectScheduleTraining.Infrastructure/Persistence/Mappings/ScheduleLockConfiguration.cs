using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    /// <summary>
    /// Configuração EF Core da entidade ScheduleLock.
    /// Define mapeamento de tabela, colunas e relacionamentos.
    /// </summary>
    public class ScheduleLockConfiguration : IEntityTypeConfiguration<ScheduleLock>
    {
        public void Configure(EntityTypeBuilder<ScheduleLock> builder)
        {
            builder.ToTable("ScheduleLocks");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.EnrollmentId)
                .IsRequired();

            builder.Property(x => x.LockStartDate)
                .IsRequired();

            builder.Property(x => x.LockEndDate)
                .IsRequired();

            builder.Property(x => x.Justification)
                .IsRequired();

            builder.Property(x => x.DocumentUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.Property(x => x.IsApproved)
                .IsRequired();

            builder.Property(x => x.Year)
                .IsRequired();

            builder.Property(x => x.Semester)
                .IsRequired();

            /// Relacionamento com Student.
            builder.HasOne(x => x.Student)
                .WithMany(x => x.ScheduleLocks)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            /// Relacionamento com Enrollment.
            builder.HasOne(x => x.Enrollment)
                .WithMany()
                .HasForeignKey(x => x.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
