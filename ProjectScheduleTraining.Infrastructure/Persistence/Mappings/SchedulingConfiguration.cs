using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    public class SchedulingConfiguration : IEntityTypeConfiguration<Scheduling>
    {
        public void Configure(EntityTypeBuilder<Scheduling> builder)
        {
            builder.ToTable("Schedulings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.JustifiedAbsenceReason)
                .HasMaxLength(500);

            builder.Property(x => x.TrainerNotes)
                .HasMaxLength(1000);

            builder.HasIndex(x => new { x.StudentId, x.ScheduleId })
                .IsUnique();

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Schedulings)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
