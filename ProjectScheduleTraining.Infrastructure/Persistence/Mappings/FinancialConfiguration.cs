using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    public class FinancialConfiguration : IEntityTypeConfiguration<Financial>
    {
        public void Configure(EntityTypeBuilder<Financial> builder)
        {
            builder.ToTable("Financials");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(300);

            builder.Property(x => x.PaymentProof)
                .HasMaxLength(500);

            builder.HasIndex(x => new { x.StudentId, x.DueDate });

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Financials)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Enrollment)
                .WithMany()
                .HasForeignKey(x => x.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
