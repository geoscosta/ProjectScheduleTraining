using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Cpf)
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Phone)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.PhotoUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Address)
                .HasMaxLength(300);

            builder.Property(x => x.EmergencyContact)
                .HasMaxLength(200);

            builder.Property(x => x.InternalNotes)
                .HasMaxLength(1000);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.HasIndex(x => x.Cpf).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
