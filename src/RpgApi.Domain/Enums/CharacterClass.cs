namespace RpgApi.Domain.Enums;

/// <summary>
/// Karaktärsklasser i RPG-spelet.
/// Vi använder en vanlig enum för enkla kategorier.
/// I SQL Server lagras detta som en INT-kolumn.
/// </summary>
public enum CharacterClass
{
    Warrior = 1,
    Mage = 2,
    Rogue = 3,
    Paladin = 4,
    Ranger = 5
}
