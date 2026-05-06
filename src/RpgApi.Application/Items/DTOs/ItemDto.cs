namespace RpgApi.Application.Items.DTOs;

public record ItemDto(
    Guid Id,
    string Name,
    string Description,
    string Type,
    int StrengthBonus,
    int IntelligenceBonus,
    int AgilityBonus,
    int DefenseBonus
);
