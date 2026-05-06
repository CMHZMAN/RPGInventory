using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RpgApi.Domain.Entities;
using RpgApi.Domain.Enums;

namespace RpgApi.Infrastructure.Persistence.Configurations;

/// <summary>
/// IEntityTypeConfiguration<T> separerar databaskonfigurationen
/// från entiteten och från DbContext. 
/// 
/// Varje entitet får sin egen konfigurationsklass = lätt att hitta
/// och ändra mappningen för en specifik tabell.
/// </summary>
public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        // Tabellnamn – vi väljer plural och explicit namn
        builder.ToTable("Characters");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Kolumnkonfiguration
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100)
            // nvarchar = stöder Unicode (svenska tecken, emoji etc.)
            .HasColumnType("nvarchar(100)");

        builder.Property(c => c.Level)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(c => c.CurrentHealth)
            .IsRequired();

        builder.Property(c => c.MaxHealth)
            .IsRequired();

        builder.Property(c => c.Strength)
            .IsRequired();

        builder.Property(c => c.Intelligence)
            .IsRequired();

        builder.Property(c => c.Agility)
            .IsRequired();

        builder.Property(c => c.Defense)
            .IsRequired();

        // Enum lagras som int i databasen (default)
        builder.Property(c => c.Class)
            .IsRequired()
            .HasConversion<int>(); // Explicit: lagra som INT

        // En-till-många: Character har många CharacterItems.
        // WithOne: varje CharacterItem tillhör en Character.
        // HasForeignKey: CharacterId är foreign key i CharacterItems-tabellen.
        // OnDelete Cascade: om karaktären raderas, raderas inventariet också.
        builder.HasMany(c => c.Inventory)
            .WithOne(ci => ci.Character)
            .HasForeignKey(ci => ci.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index förbättrar sökhastigheten för vanliga queries
        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.Class);
    }
}
