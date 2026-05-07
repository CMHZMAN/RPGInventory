using MediatR;
using RpgApi.Application.Characters.DTOs;
using RpgApi.Application.Common.Exceptions;
using RpgApi.Application.Common.Mappings;
using RpgApi.Domain.Entities;
using RpgApi.Domain.Interfaces;

namespace RpgApi.Application.Characters.Commands;

/// <summary>
/// Skapar en ny karaktär.
/// Mönster: Validera → Skapa domänobjekt → Spara → Returnera DTO.
/// </summary>
public class CreateCharacterCommandHandler
    : IRequestHandler<CreateCharacterCommand, CharacterDto>
{
    private readonly IUnitOfWork _uow;

    public CreateCharacterCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CharacterDto> Handle(
        CreateCharacterCommand request,
        CancellationToken cancellationToken)
    {
        // Domänentiteten validerar egna regler i fabriksmetoden.
        // Vi behöver inte validera namn-längd här – det är domänens ansvar.
        var character = Character.Create(request.Name, request.Class);

        await _uow.Characters.AddAsync(character, cancellationToken);

        // SaveChangesAsync skriver till databasen i en transaktion.
        await _uow.SaveChangesAsync(cancellationToken);

        return character.ToDto();
    }
}

/// <summary>
/// Uppdaterar karaktärens namn.
/// Mönster: Hämta → Verifiera existens → Anropa domänmetod → Spara → Returnera.
/// </summary>
public class UpdateCharacterNameCommandHandler
    : IRequestHandler<UpdateCharacterNameCommand, CharacterDto>
{
    private readonly IUnitOfWork _uow;

    public UpdateCharacterNameCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CharacterDto> Handle(
        UpdateCharacterNameCommand request,
        CancellationToken cancellationToken)
    {
        var character = await _uow.Characters
            .GetByIdAsync(request.CharacterId, cancellationToken)
            ?? throw new NotFoundException(nameof(Character), request.CharacterId);

        // Vi anropar domänmetoden – entiteten ansvarar för sin logik.
        character.UpdateName(request.NewName);

        _uow.Characters.Update(character);
        await _uow.SaveChangesAsync(cancellationToken);

        return character.ToDto();
    }
}

public class LevelUpCharacterCommandHandler
    : IRequestHandler<LevelUpCharacterCommand, CharacterDto>
{
    private readonly IUnitOfWork _uow;

    public LevelUpCharacterCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CharacterDto> Handle(
        LevelUpCharacterCommand request,
        CancellationToken cancellationToken)
    {
        var character = await _uow.Characters
            .GetByIdAsync(request.CharacterId, cancellationToken)
            ?? throw new NotFoundException(nameof(Character), request.CharacterId);

        character.LevelUp();

        _uow.Characters.Update(character);
        await _uow.SaveChangesAsync(cancellationToken);

        return character.ToDto();
    }
}

public class AddItemToCharacterCommandHandler
    : IRequestHandler<AddItemToCharacterCommand, CharacterDto>
{
    private readonly IUnitOfWork _uow;

    public AddItemToCharacterCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CharacterDto> Handle(
        AddItemToCharacterCommand request,
        CancellationToken cancellationToken)
    {
        // Hämta med inventory (eager load) så att vi kan kontrollera
        // affärsregeln "max 20 föremål" och "redan i inventory".
        var character = await _uow.Characters
            .GetWithInventoryAsync(request.CharacterId, cancellationToken)
            ?? throw new NotFoundException(nameof(Character), request.CharacterId);

        var item = await _uow.Items
            .GetByIdAsync(request.ItemId, cancellationToken)
            ?? throw new NotFoundException(nameof(Item), request.ItemId);

        // Domänentiteten kastar InvalidOperationException vid regelbrott.
        // Vi låter den bubbla upp till API-lagrets exception-hanterare.
        character.AddItemToInventory(item);

        // Hämta det nyss skapade CharacterItem-objektet ur inventariet
        // och registrera det EXPLICIT i EF Cores Change Tracker som Added.
        // EF Core detekterar inte alltid additions till privata backing fields
        // automatiskt – explicit AddAsync garanterar korrekt INSERT.
        var newCharacterItem = character.Inventory.First(ci => ci.ItemId == item.Id);
        await _uow.CharacterItems.AddAsync(newCharacterItem, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        // Hämta igen med inventory så att Item-navigationsproperty är laddad i DTOn.
        var updated = await _uow.Characters
            .GetWithInventoryAsync(request.CharacterId, cancellationToken);

        return updated!.ToDto();
    }
}

public class RemoveItemFromCharacterCommandHandler
    : IRequestHandler<RemoveItemFromCharacterCommand, CharacterDto>
{
    private readonly IUnitOfWork _uow;

    public RemoveItemFromCharacterCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CharacterDto> Handle(
        RemoveItemFromCharacterCommand request,
        CancellationToken cancellationToken)
    {
        var character = await _uow.Characters
            .GetWithInventoryAsync(request.CharacterId, cancellationToken)
            ?? throw new NotFoundException(nameof(Character), request.CharacterId);

        var removed = character.RemoveItemFromInventory(request.ItemId);

        if (!removed)
            throw new NotFoundException("Item i inventariet", request.ItemId);

        // Samma princip: CharacterItem är spårad som Deleted av Change Tracker
        // när vi tar bort den från listan. Inget Update() behövs.
        await _uow.SaveChangesAsync(cancellationToken);

        var updated = await _uow.Characters
            .GetWithInventoryAsync(request.CharacterId, cancellationToken);

        return updated!.ToDto();
    }
}

public class DeleteCharacterCommandHandler
    : IRequestHandler<DeleteCharacterCommand>
{
    private readonly IUnitOfWork _uow;

    public DeleteCharacterCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(
        DeleteCharacterCommand request,
        CancellationToken cancellationToken)
    {
        var character = await _uow.Characters
            .GetByIdAsync(request.CharacterId, cancellationToken)
            ?? throw new NotFoundException(nameof(Character), request.CharacterId);

        _uow.Characters.Delete(character);
        await _uow.SaveChangesAsync(cancellationToken);
    }
}
