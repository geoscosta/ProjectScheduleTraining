using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(x => x.Id);

        /// Dados pessoais básicos.
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

        /// Novos campos obrigatórios do contrato.
        builder.Property(x => x.Profession)
            .HasMaxLength(100);

        builder.Property(x => x.MaritalStatus);

        builder.Property(x => x.IdentityDocument)
            .HasMaxLength(20);

        /// Endereço estruturado.
        builder.Property(x => x.Street)
            .HasMaxLength(200);

        builder.Property(x => x.AddressNumber)
            .HasMaxLength(20);

        builder.Property(x => x.Complement)
            .HasMaxLength(100);

        builder.Property(x => x.District)
            .HasMaxLength(100);

        builder.Property(x => x.City)
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .HasMaxLength(2);

        builder.Property(x => x.ZipCode)
            .HasMaxLength(9);

        /// Responsável legal para menores de idade.
        builder.Property(x => x.GuardianName)
            .HasMaxLength(150);

        builder.Property(x => x.GuardianCpf)
            .HasMaxLength(11);

        /// Documentação e termos.
        builder.Property(x => x.RegistrationFeePaid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.HealthCertificateExpiresAt);

        builder.Property(x => x.ImageRightsAccepted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.InternalRegulationAccepted)
            .IsRequired()
            .HasDefaultValue(false);

        /// Notas e status.
        builder.Property(x => x.EmergencyContact)
            .HasMaxLength(200);

        builder.Property(x => x.InternalNotes)
            .HasMaxLength(1000);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        /// Índices únicos.
        builder.HasIndex(x => x.Cpf).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}