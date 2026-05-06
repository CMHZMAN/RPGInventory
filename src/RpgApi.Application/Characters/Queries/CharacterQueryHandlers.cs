using MediatR;
using RpgApi.Application.Characters.DTOs;
using RpgApi.Application.Common.Mappings;
using RpgApi.Domain.Interfaces;

namespace RpgApi.Application.Characters.Queries;

/// <summary>
/// Handler = klassen som UTFÖR det ett meddelande (Query/Command) beskriver.
///
/// MediatR kopplar automatiskt ihop Query → Handler via IRequestHandler&lt;TRequest, TResponse&gt;.
/// Vi behöver aldrig anropa handlens direkt – vi skickar bara queryn via mediator.Send().
///
/// Flöde för en HTTP GET /characters/{id}:
///   Controller → mediator.Send(query) → MediatR → Handler → Repository → Databas
///
/// IRequestHandler&lt;GetCharacterByIdQuery, CharacterDto?&gt; = den här handlens hanterar
/// GetCharacterByIdQuery och returnerar CharacterDto? (null om inte hittas).
/// </summary>
public class GetCharacterByIdQueryHandler
    : IRequestHandler<GetCharacterByIdQuery, CharacterDto?>
{
    private readonly IUnitOfWork _uow;

    // Constructor Injection – DI-containern injicerar IUnitOfWork automatiskt.
    public GetCharacterByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<CharacterDto?> Handle(
        GetCharacterByIdQuery request,
        CancellationToken cancellationToken)
    {
        var character = await _uow.Characters
            .GetWithInventoryAsync(request.CharacterId, cancellationToken);

        // Null-conditional: om character är null returneras null direkt.
        // Controller avgör sedan om det ska bli 404.
        return character?.ToDto();
    }
}

public class GetAllCharactersQueryHandler
    : IRequestHandler<GetAllCharactersQuery, IEnumerable<CharacterDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllCharactersQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<CharacterDto>> Handle(
        GetAllCharactersQuery request,
        CancellationToken cancellationToken)
    {
        var characters = await _uow.Characters.GetAllAsync(cancellationToken);

        // LINQ Select = transformera varje entitet till DTO.
        // ToList() materialiserar IEnumerable direkt – undviker lazy evaluation-problem.
        return characters.Select(c => c.ToDto()).ToList();
    }
}
