using MediatR;
using RpgApi.Application.Characters.DTOs;
using RpgApi.Domain.Enums;

namespace RpgApi.Application.Characters.Commands;

/// <summary>
/// Commands beskriver en INTENTION att förändra systemet.
/// Till skillnad från Queries förväntas Commands ha sidoeffekter (skriva till DB).
///
/// Namnkonvention: [Verb][Entitet]Command
/// Returnerar: antingen void (IRequest) eller en DTO med det skapade/uppdaterade objektet.
/// Att returnera den skapade resursen direkt sparar en extra roundtrip för klienten.
/// </summary>
public record CreateCharacterCommand(
    string Name,
    CharacterClass Class
) : IRequest<CharacterDto>;

public record UpdateCharacterNameCommand(
    Guid CharacterId,
    string NewName
) : IRequest<CharacterDto>;

public record LevelUpCharacterCommand(
    Guid CharacterId
) : IRequest<CharacterDto>;

public record AddItemToCharacterCommand(
    Guid CharacterId,
    Guid ItemId
) : IRequest<CharacterDto>;

public record RemoveItemFromCharacterCommand(
    Guid CharacterId,
    Guid ItemId
) : IRequest<CharacterDto>;

/// <summary>
/// IRequest (utan typparameter) = kommandot returnerar ingenting (void).
/// Används när vi inte behöver bekräftelse utöver HTTP 204 No Content.
/// </summary>
public record DeleteCharacterCommand(Guid CharacterId) : IRequest;
