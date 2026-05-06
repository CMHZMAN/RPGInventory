using MediatR;
using RpgApi.Application.Characters.DTOs;

namespace RpgApi.Application.Characters.Queries;

/// <summary>
/// Query = ett meddelande som ENBART läser data.
/// IRequest&lt;T&gt; = MediatR vet att denna query förväntar sig svarstypen T.
///
/// Record passar perfekt för CQRS-meddelanden:
/// - Immutable (kan inte ändras efter skapande)
/// - Värdesemantik (två queries med samma CharacterId är lika)
/// - Kompakt syntax
/// </summary>
public record GetCharacterByIdQuery(Guid CharacterId) : IRequest<CharacterDto?>;

public record GetAllCharactersQuery : IRequest<IEnumerable<CharacterDto>>;
