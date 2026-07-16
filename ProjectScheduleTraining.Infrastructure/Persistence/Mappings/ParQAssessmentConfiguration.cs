using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    /// <summary>
    /// Configuração EF Core da entidade ParQAssessment.
    /// Define mapeamento de tabela, colunas e relacionamentos.
    /// </summary>
    public class ParQAssessmentConfiguration : IEntityTypeConfiguration<ParQAssessment>
    {
        public void Configure(EntityTypeBuilder<ParQAssessment> builder)
        {
            builder.ToTable("ParQAssessments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.AssessmentDate)
                .IsRequired();

            builder.Property(x => x.Question1).IsRequired();
            builder.Property(x => x.Question2).IsRequired();
            builder.Property(x => x.Question3).IsRequired();
            builder.Property(x => x.Question4).IsRequired();
            builder.Property(x => x.Question5).IsRequired();
            builder.Property(x => x.Question6).IsRequired();
            builder.Property(x => x.Question7).IsRequired();

            builder.Property(x => x.IsCleared).IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            /// Relacionamento com Student.
            builder.HasOne(x => x.Student)
                .WithMany(x => x.ParQAssessments)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
