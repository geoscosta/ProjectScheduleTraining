using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.ToTable("Plans");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.WeeklyFrequency)
                .IsRequired();

            builder.Property(x => x.DurationMonths)
                .IsRequired();
        }
    }
}
