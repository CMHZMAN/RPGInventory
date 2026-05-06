using RpgApi.Domain.Enums;

namespace RpgApi.Application.Characters.DTOs;

/// <summary>
/// DTO (Data Transfer Object) – vad vi RETURNERAR till klienten.
/// 
/// Vi exponerar ALDRIG domänentiteten direkt mot API:et av flera skäl:
/// 1. Säkerhet: domänentiteten kan ha känsliga fält
/// 2. Kontrakt: vi kan ändra domänen utan att påverka API-kontraktet
/// 3. Form: vi kan forma svaret exakt som klienten behöver det
/// 
/// Record är perfekt för DTOs: immutable, strukturell likhet,
/// kompakt syntax med primary constructor.
/// </summary>
public record CharacterDto(
    Guid Id,
    string Name,
    int Level,
    int CurrentHealth,
    int MaxHealth,
    int Strength,
    int Intelligence,
    int Agility,
    int Defense,
    string Class,
    bool IsAlive,
    IReadOnlyList<InventoryItemDto> Inventory
);

public record InventoryItemDto(
    Guid CharacterItemId,
    Guid ItemId,
    string Name,
    string Description,
    string Type,
    bool IsEquipped,
    int StrengthBonus,
    int IntelligenceBonus,
    int AgilityBonus,
    int DefenseBonus
);
