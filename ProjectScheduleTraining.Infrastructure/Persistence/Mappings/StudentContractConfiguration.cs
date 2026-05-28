using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    /// <summary>
    /// Configuração EF Core da entidade StudentContract.
    /// Define mapeamento de tabela, colunas e relacionamentos.
    /// </summary>
    public class StudentContractConfiguration : IEntityTypeConfiguration<StudentContract>
    {
        public void Configure(EntityTypeBuilder<StudentContract> builder)
        {
            builder.ToTable("StudentContracts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StudentId)
                .IsRequired();

            builder.Property(x => x.EnrollmentId)
                .IsRequired();

            builder.Property(x => x.SignedAt)
                .IsRequired();

            builder.Property(x => x.SignatureHash)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.SignatureIp)
                .HasMaxLength(45);

            builder.Property(x => x.ContractContent)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.PdfUrl)
                .HasMaxLength(500);

            builder.Property(x => x.IsSigned)
                .IsRequired();

            /// Relacionamento com Student.
            builder.HasOne(x => x.Student)
                .WithMany(x => x.Contracts)
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
