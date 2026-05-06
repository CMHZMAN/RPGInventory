using RpgApi.Domain.Common;
using RpgApi.Domain.Enums;

namespace RpgApi.Domain.Entities;

/// <summary>
/// Item är ett föremål i spelet (svärd, rustning, trolldryck etc.).
/// 
/// Rich Domain Model: klassen innehåller BETEENDE, inte bara data.
/// En "Anemic Domain Model" (antimönster) skulle bara ha get/set-properties
/// och all logik i services – det är vi vill undvika.
/// </summary>
public class Item : BaseEntity
{
    // Private set = EF Core kan sätta via reflection, men utomstående
    // kod måste gå via metoderna. Det skyddar invarianter (regler).
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public ItemType Type { get; private set; }

    /// <summary>
    /// Bonus-statistik som föremålet ger bäraren.
    /// Owned Entity i EF Core – lagras i samma tabell som Item
    /// men representerar ett eget Value Object i domänen.
    /// </summary>
    public int StrengthBonus { get; private set; }
    public int IntelligenceBonus { get; private set; }
    public int AgilityBonus { get; private set; }
    public int DefenseBonus { get; private set; }

    // Krävs av EF Core – EF Core skapar objekt utan att köra vår konstruktor
    // när den läser från databasen. 'protected' hindrar att vi råkar
    // använda den i applikationskoden.
    protected Item() { }

    // Privat konstruktor = vi tvingar användning av fabriksmetoden.
    private Item(string name, string description, ItemType type,
        int strengthBonus, int intelligenceBonus, int agilityBonus, int defenseBonus)
    {
        Name = name;
        Description = description;
        Type = type;
        StrengthBonus = strengthBonus;
        IntelligenceBonus = intelligenceBonus;
        AgilityBonus = agilityBonus;
        DefenseBonus = defenseBonus;
    }

    /// <summary>
    /// Statisk fabriksmetod (Factory Method Pattern).
    /// Fördelar:
    /// 1. Beskrivande namn (Create är tydligare än 'new Item()')
    /// 2. Validering sker INNAN objektet existerar
    /// 3. Kan returnera null eller subtyper utan att ändra API:et
    /// </summary>
    public static Item Create(string name, string description, ItemType type,
        int strengthBonus = 0, int intelligenceBonus = 0,
        int agilityBonus = 0, int defenseBonus = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Föremålsnamn får inte vara tomt.", nameof(name));

        return new Item(name, description, type,
            strengthBonus, intelligenceBonus, agilityBonus, defenseBonus);
    }

    /// <summary>
    /// Domänmetod för att uppdatera föremålet.
    /// Istället för att exponera set-properties låter vi entiteten
    /// kontrollera sin egen tillståndsförändring.
    /// </summary>
    public void Update(string name, string description,
        int strengthBonus, int intelligenceBonus, int agilityBonus, int defenseBonus)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Föremålsnamn får inte vara tomt.", nameof(name));

        Name = name;
        Description = description;
        StrengthBonus = strengthBonus;
        IntelligenceBonus = intelligenceBonus;
        AgilityBonus = agilityBonus;
        DefenseBonus = defenseBonus;
    }
}
