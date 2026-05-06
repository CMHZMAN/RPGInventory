using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RpgApi.Domain.Entities;

namespace RpgApi.Infrastructure.Persistence.Configurations;

public class CharacterItemConfiguration : IEntityTypeConfiguration<CharacterItem>
{
    public void Configure(EntityTypeBuilder<CharacterItem> builder)
    {
        builder.ToTable("CharacterItems");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.IsEquipped)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ci => ci.AcquiredAt)
            .IsRequired()
            // datetime2 = SQL Servers precisare datumtyp (vs datetime).
            // Rekommenderas för nya projekt i SQL Server 2022.
            .HasColumnType("datetime2");

        // En CharacterItem tillhör ett Item – om Item raderas, ta bort
        // CharacterItem också (Cascade). Alternativt Restrict för att
        // hindra borttagning av items som är i bruk.
        builder.HasOne(ci => ci.Item)
            .WithMany()
            .HasForeignKey(ci => ci.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Sammansatt index: snabbare sökning efter en karaktärs specifika item
        builder.HasIndex(ci => new { ci.CharacterId, ci.ItemId }).IsUnique();
    }
}
