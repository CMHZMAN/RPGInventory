using RpgApi.Application.Characters.DTOs;
using RpgApi.Application.Items.DTOs;
using RpgApi.Domain.Entities;

namespace RpgApi.Application.Common.Mappings;

/// <summary>
/// Statisk mappningsklass – konverterar domänentiteter till DTOs.
///
/// Vi väljer manuell mappning framför t.ex. AutoMapper för att:
/// 1. Göra mappningen explicit och sökbar (Ctrl+Click fungerar)
/// 2. Undvika "magic" som döljer fel till runtime
/// 3. Kompilatorn fångar direkt om en property byts namn
///
/// Extension methods på domänentiteterna håller anropssyntaxen ren:
///   character.ToDto()  istället för  MappingHelper.Map(character)
/// </summary>
public static class MappingExtensions
{
    public static CharacterDto ToDto(this Character character) =>
        new(
            Id:            character.Id,
            Name:          character.Name,
            Level:         character.Level,
            CurrentHealth: character.CurrentHealth,
            MaxHealth:     character.MaxHealth,
            Strength:      character.Strength,
            Intelligence:  character.Intelligence,
            Agility:       character.Agility,
            Defense:       character.Defense,
            Class:         character.Class.ToString(),
            IsAlive:       character.IsAlive,
            Inventory:     character.Inventory
                               .Select(ci => ci.ToDto())
                               .ToList()
                               .AsReadOnly()
        );

    public static InventoryItemDto ToDto(this CharacterItem ci) =>
        new(
            CharacterItemId:   ci.Id,
            ItemId:            ci.ItemId,
            Name:              ci.Item?.Name        ?? string.Empty,
            Description:       ci.Item?.Description ?? string.Empty,
            Type:              ci.Item?.Type.ToString() ?? string.Empty,
            IsEquipped:        ci.IsEquipped,
            StrengthBonus:     ci.Item?.StrengthBonus     ?? 0,
            IntelligenceBonus: ci.Item?.IntelligenceBonus ?? 0,
            AgilityBonus:      ci.Item?.AgilityBonus      ?? 0,
            DefenseBonus:      ci.Item?.DefenseBonus      ?? 0
        );

    public static ItemDto ToDto(this Item item) =>
        new(
            Id:               item.Id,
            Name:             item.Name,
            Description:      item.Description,
            Type:             item.Type.ToString(),
            StrengthBonus:    item.StrengthBonus,
            IntelligenceBonus:item.IntelligenceBonus,
            AgilityBonus:     item.AgilityBonus,
            DefenseBonus:     item.DefenseBonus
        );
}
