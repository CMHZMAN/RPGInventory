using RpgApi.Domain.Common;
using RpgApi.Domain.Enums;

namespace RpgApi.Domain.Entities;

/// <summary>
/// Character är spelets huvudentitet – en karaktär som spelare styr.
/// 
/// Notera: Character äger en lista av CharacterItems (inventory).
/// Det här är ett "Aggregate Root" i DDD (Domain-Driven Design):
/// Character kontrollerar allt som händer med sitt inventory.
/// Ingenting utifrån ska direkt manipulera CharacterItem-listan.
/// </summary>
public class Character : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public int Level { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public int Strength { get; private set; }
    public int Intelligence { get; private set; }
    public int Agility { get; private set; }
    public int Defense { get; private set; }
    public CharacterClass Class { get; private set; }
    public bool IsAlive => CurrentHealth > 0;

    // Backing field-mönster: '_inventory' är den interna listan.
    // Utomstående ser bara en IReadOnlyCollection – de kan läsa men inte
    // direkt lägga till/ta bort. All mutation går via våra metoder.
    private readonly List<CharacterItem> _inventory = [];
    public IReadOnlyCollection<CharacterItem> Inventory => _inventory.AsReadOnly();

    protected Character() { }

    private Character(string name, CharacterClass characterClass,
        int maxHealth, int strength, int intelligence, int agility, int defense)
    {
        Name = name;
        Class = characterClass;
        Level = 1;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth; // Börjar med full hälsa
        Strength = strength;
        Intelligence = intelligence;
        Agility = agility;
        Defense = defense;
    }

    /// <summary>
    /// Skapar en ny karaktär med klasspecifika grundvärden.
    /// 
    /// Switch expression (C# 8+) – modernare och mer kompakt än switch-statement.
    /// Pattern matching på enum-värdet ger varje klass unika stats.
    /// </summary>
    public static Character Create(string name, CharacterClass characterClass)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Karaktärsnamn får inte vara tomt.", nameof(name));

        // Destrukturering med tuplar – returnerar flera värden på ett rent sätt
        var (maxHp, str, intel, agi, def) = characterClass switch
        {
            CharacterClass.Warrior  => (150, 20, 5,  10, 15),
            CharacterClass.Mage     => (80,  5,  25, 8,  5),
            CharacterClass.Rogue    => (100, 15, 10, 25, 8),
            CharacterClass.Paladin  => (120, 15, 15, 8,  20),
            CharacterClass.Ranger   => (110, 12, 12, 20, 10),
            // '_' är discard/default-arm – hanterar okända värden
            _ => throw new ArgumentOutOfRangeException(nameof(characterClass))
        };

        return new Character(name, characterClass, maxHp, str, intel, agi, def);
    }

    /// <summary>
    /// Lägg till föremål i inventariet.
    /// Affärsregel: en karaktär kan max bära 20 föremål.
    /// Regeln sitter i DOMÄNEN, inte i en service eller controller.
    /// </summary>
    public void AddItemToInventory(Item item)
    {
        if (_inventory.Count >= 20)
            throw new InvalidOperationException(
                $"{Name} kan inte bära fler föremål. Inventariet är fullt (max 20).");

        if (_inventory.Any(ci => ci.ItemId == item.Id))
            throw new InvalidOperationException(
                $"{Name} bär redan föremålet '{item.Name}'.");

        _inventory.Add(CharacterItem.Create(Id, item.Id));
    }

    /// <summary>
    /// Ta bort föremål från inventariet.
    /// Notera att vi returnerar bool – vi kastar inget undantag om
    /// föremålet inte finns, det är inte ett felläge utan ett normalt utfall.
    /// </summary>
    public bool RemoveItemFromInventory(Guid itemId)
    {
        var characterItem = _inventory.FirstOrDefault(ci => ci.ItemId == itemId);
        if (characterItem is null) return false;

        _inventory.Remove(characterItem);
        return true;
    }

    /// <summary>
    /// Karaktären tar skada. Hälsa kan aldrig bli negativ (Min-klämning).
    /// Affärslogik sitter i entiteten – inte i en SkadaService.
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage), "Skada kan inte vara negativ.");

        CurrentHealth = Math.Max(0, CurrentHealth - damage);
    }

    /// <summary>
    /// Läk karaktären. Hälsa kan aldrig överskrida MaxHealth.
    /// </summary>
    public void Heal(int amount)
    {
        if (!IsAlive)
            throw new InvalidOperationException("Kan inte läka en dead karaktär.");

        CurrentHealth = Math.Min(MaxHealth, CurrentHealth + amount);
    }

    /// <summary>
    /// Level up – ökar nivå och förbättrar stats baserat på klass.
    /// </summary>
    public void LevelUp()
    {
        Level++;

        // Klassens primärstat ökar mer vid level up
        (Strength, Intelligence, Agility, Defense, MaxHealth) = Class switch
        {
            CharacterClass.Warrior  => (Strength + 4, Intelligence + 1, Agility + 2, Defense + 3, MaxHealth + 20),
            CharacterClass.Mage     => (Strength + 1, Intelligence + 5, Agility + 2, Defense + 1, MaxHealth + 10),
            CharacterClass.Rogue    => (Strength + 3, Intelligence + 2, Agility + 5, Defense + 2, MaxHealth + 15),
            CharacterClass.Paladin  => (Strength + 3, Intelligence + 3, Agility + 1, Defense + 4, MaxHealth + 18),
            CharacterClass.Ranger   => (Strength + 2, Intelligence + 2, Agility + 4, Defense + 2, MaxHealth + 15),
            _ => throw new InvalidOperationException("Okänd karaktärsklass.")
        };

        // Återställ hälsa till nya maxvärdet vid level up
        CurrentHealth = MaxHealth;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Karaktärsnamn får inte vara tomt.", nameof(name));
        Name = name;
    }
}
