using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectScheduleTraining.Domain.Entities;

namespace ProjectScheduleTraining.Infrastructure.Persistence.Mappings
{
    /// <summary>
    /// Configuração do mapeamento da entidade RefreshToken para o banco de dados.
    /// </summary>
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Token)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.HasIndex(x => x.Token).IsUnique();
        }
    }
}
