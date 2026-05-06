using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RpgApi.Domain.Entities;
using RpgApi.Domain.Enums;

namespace RpgApi.Infrastructure.Persistence.Configurations;

/// <summary>
/// Seed-data konfigureras med HasData() i EF Core.
/// EF Core sköter INSERT vid migration om raden saknas.
///
/// Vi använder fasta Guids (inte Guid.NewGuid()) så att EF Core
/// alltid genererar identiska migrations – annars får vi nya INSERT
/// vid varje "add-migration" och dubbel data i databasen.
/// </summary>
public class SeedDataConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasData(
            CreateItem(new Guid("11111111-0000-0000-0000-000000000001"),
                "Järnsvärd", "Ett vanligt järnsvärd.", ItemType.Weapon,
                strengthBonus: 5),

            CreateItem(new Guid("11111111-0000-0000-0000-000000000002"),
                "Trollstav", "En stav laddad med magisk energi.", ItemType.Weapon,
                intelligenceBonus: 8),

            CreateItem(new Guid("11111111-0000-0000-0000-000000000003"),
                "Läderharnesk", "Lätt rustning som inte hämmar rörelseförmågan.", ItemType.Armor,
                defenseBonus: 5, agilityBonus: 2),

            CreateItem(new Guid("11111111-0000-0000-0000-000000000004"),
                "Plåtrustning", "Tung och skyddande stålrustning.", ItemType.Armor,
                defenseBonus: 12, strengthBonus: 2),

            CreateItem(new Guid("11111111-0000-0000-0000-000000000005"),
                "Helbredelsedryck", "Återställer 50 hälsopoäng.", ItemType.Potion),

            CreateItem(new Guid("11111111-0000-0000-0000-000000000006"),
                "Styrkedryck", "Ger +10 styrka i 1 timme.", ItemType.Potion,
                strengthBonus: 10),

            CreateItem(new Guid("11111111-0000-0000-0000-000000000007"),
                "Smygdolk", "En lätt dolk perfekt för lönnmördare.", ItemType.Weapon,
                strengthBonus: 3, agilityBonus: 4),

            CreateItem(new Guid("11111111-0000-0000-0000-000000000008"),
                "Visdomsamulett", "Ökar bärarens intelligens.", ItemType.Accessory,
                intelligenceBonus: 6)
        );
    }

    // Hjälpmetod för att slippa upprepa 'new Item(...)'-syntax.
    // Vi använder object initializer med kolumnnamn eftersom EF Core
    // behöver sätta de private properties via shadow state vid seed.
    private static object CreateItem(Guid id, string name, string description,
        ItemType type, int strengthBonus = 0, int intelligenceBonus = 0,
        int agilityBonus = 0, int defenseBonus = 0) => new
        {
            Id = id,
            Name = name,
            Description = description,
            Type = type,
            StrengthBonus = strengthBonus,
            IntelligenceBonus = intelligenceBonus,
            AgilityBonus = agilityBonus,
            DefenseBonus = defenseBonus
        };
}
