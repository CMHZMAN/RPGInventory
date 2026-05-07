using MediatR;
using RpgApi.Application.Items.DTOs;
using RpgApi.Domain.Enums;

namespace RpgApi.Application.Items.Commands;

public record CreateItemCommand(
    string Name,
    string Description,
    ItemType Type,
    int StrengthBonus,
    int IntelligenceBonus,
    int AgilityBonus,
    int DefenseBonus
) : IRequest<ItemDto>;

public record UpdateItemCommand(
    Guid ItemId,
    string Name,
    string Description,
    int StrengthBonus,
    int IntelligenceBonus,
    int AgilityBonus,
    int DefenseBonus
) : IRequest<ItemDto>;

public record DeleteItemCommand(Guid ItemId) : IRequest;
