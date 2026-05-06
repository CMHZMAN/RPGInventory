namespace RpgApi.Domain.ValueObjects;

/// <summary>
/// Ett Value Object representerar ett VÄRDE, inte en identitet.
/// Två Stats-objekt med samma värden är IDENTISKA – till skillnad
/// från entiteter som identifieras av sitt Id.
/// 
/// Exempel: 100 kr == 100 kr (värde). Men Karaktär #1 != Karaktär #2
/// även om de råkar ha samma namn.
/// 
/// Vi använder C# record för Value Objects eftersom records automatiskt
/// implementerar strukturell likhet (Equals, GetHashCode, ==).
/// 
/// 'sealed' hindrar arv – Value Objects ska inte ärvas från.
/// </summary>
public sealed record CharacterStats
{
    public int MaxHealth { get; }
    public int Strength { get; }
    public int Intelligence { get; }
    public int Agility { get; }
    public int Defense { get; }

    // Privat konstruktor + statisk fabriksmetod = vi kontrollerar
    // hur objekt skapas och kan validera redan vid skapandet.
    private CharacterStats(int maxHealth, int strength, int intelligence,
        int agility, int defense)
    {
        MaxHealth = maxHealth;
        Strength = strength;
        Intelligence = intelligence;
        Agility = agility;
        Defense = defense;
    }

    /// <summary>
    /// Fabriksmetod som validerar data INNAN objektet skapas.
    /// Principen: ett ogiltigt objekt ska aldrig kunna existera.
    /// </summary>
    public static CharacterStats Create(int maxHealth, int strength,
        int intelligence, int agility, int defense)
    {
        // Guard clauses – kontrollera villkor tidigt och kasta undantag.
        // Tydligare än nästlade if-satser.
        if (maxHealth <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxHealth),
                "MaxHealth måste vara större än 0.");
        if (strength < 0 || intelligence < 0 || agility < 0 || defense < 0)
            throw new ArgumentOutOfRangeException("Statistikvärden kan inte vara negativa.");

        return new CharacterStats(maxHealth, strength, intelligence, agility, defense);
    }

    /// <summary>
    /// Skapar standardvärden för en ny karaktär.
    /// Static factory methods med beskrivande namn = self-documenting code.
    /// </summary>
    public static CharacterStats Default() =>
        Create(maxHealth: 100, strength: 10, intelligence: 10, agility: 10, defense: 5);

    /// <summary>
    /// Oföränderlighet (Immutability) – ett Value Object ändras ALDRIG.
    /// Istället returnerar vi ett NYTT objekt med uppdaterade värden.
    /// Detta undviker oväntade sidoeffekter (side effects).
    /// </summary>
    public CharacterStats WithStrengthBonus(int bonus) =>
        Create(MaxHealth, Strength + bonus, Intelligence, Agility, Defense);

    public CharacterStats WithIntelligenceBonus(int bonus) =>
        Create(MaxHealth, Strength, Intelligence + bonus, Agility, Defense);
}
