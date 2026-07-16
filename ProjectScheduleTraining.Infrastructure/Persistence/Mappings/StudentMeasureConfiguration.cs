using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    /// <summary>
    /// Configuração EF Core da entidade StudentMeasure.
    /// Define mapeamento de tabela, colunas e relacionamentos.
    /// </summary>
    public class StudentMeasureConfiguration : IEntityTypeConfiguration<StudentMeasure>
    {
        public void Configure(EntityTypeBuilder<StudentMeasure> builder)
        {
            builder.ToTable("StudentMeasures");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.MeasureDate)
                .IsRequired();

            builder.Property(x => x.Weight)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.Height)
                .IsRequired()
                .HasColumnType("decimal(4,2)");

            builder.Property(x => x.Bmi)
                .IsRequired()
                .HasColumnType("decimal(4,2)");

            builder.Property(x => x.ChestCircumference)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.WaistCircumference)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.HipCircumference)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.ArmCircumference)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.ThighCircumference)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.CalfCircumference)
                .HasColumnType("decimal(5,2)");

            builder.Property(x => x.BodyFatPercentage)
                .HasColumnType("decimal(4,2)");

            builder.Property(x => x.LeanMassPercentage)
                .HasColumnType("decimal(4,2)");

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            /// Relacionamento com Student.
            builder.HasOne(x => x.Student)
                .WithMany(x => x.Measures)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
