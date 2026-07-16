using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.ToTable("Schedules");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Date)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime)
                .IsRequired();

            builder.Property(x => x.MaxCapacity)
                .IsRequired()
                .HasDefaultValue(5);

            builder.Property(x => x.OccupiedSlots)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.HasIndex(x => new { x.Date, x.StartTime });

            builder.HasMany(x => x.Schedulings)
                .WithOne(x => x.Schedule)
                .HasForeignKey(x => x.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
