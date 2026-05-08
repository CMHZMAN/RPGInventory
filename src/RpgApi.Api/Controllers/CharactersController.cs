using MediatR;
<<<<<<< HEAD
=======
using Microsoft.AspNetCore.Authorization;
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
using Microsoft.AspNetCore.Mvc;
using RpgApi.Application.Characters.Commands;
using RpgApi.Application.Characters.DTOs;
using RpgApi.Application.Characters.Queries;

namespace RpgApi.Api.Controllers;

<<<<<<< HEAD
/// <summary>
/// ApiController + Route sätter bas-URL till /api/characters.
///
/// [ApiController] aktiverar automatisk modellvalidering och
/// problemdetails-svar (fast vi hanterar undantag i middleware).
///
/// Controllern är TUNN – den delegerar allt till MediatR.
/// Ingen affärslogik, ingen databaslogik här. Bara:
///   1. Ta emot HTTP-request
///   2. Skapa ett Command/Query
///   3. Skicka via _mediator.Send()
///   4. Returnera rätt HTTP-svar
/// </summary>
[ApiController]
[Route("api/[controller]")]
=======
/// <summary>CRUD-operationer för karaktärer och inventariehantering.</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
public class CharactersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CharactersController(IMediator mediator) => _mediator = mediator;

    // GET /api/characters
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CharacterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var characters = await _mediator.Send(new GetAllCharactersQuery(), cancellationToken);
        return Ok(characters);
    }

    // GET /api/characters/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var character = await _mediator.Send(new GetCharacterByIdQuery(id), cancellationToken);

        // Null → 404, annars 200 med DTO.
        // Undantaget kastas i middleware – här är det null-pattern vi hanterar.
        return character is null ? NotFound() : Ok(character);
    }

    // POST /api/characters
    [HttpPost]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCharacterCommand command,
        CancellationToken cancellationToken)
    {
        var character = await _mediator.Send(command, cancellationToken);

        // 201 Created med Location-header som pekar på den nya resursen.
        // Best practice för POST i REST.
        return CreatedAtAction(nameof(GetById), new { id = character.Id }, character);
    }

    // PUT /api/characters/{id}/name
    [HttpPut("{id:guid}/name")]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateName(
        Guid id,
        [FromBody] UpdateCharacterNameRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCharacterNameCommand(id, request.NewName);
        var character = await _mediator.Send(command, cancellationToken);
        return Ok(character);
    }

    // POST /api/characters/{id}/levelup
    [HttpPost("{id:guid}/levelup")]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LevelUp(Guid id, CancellationToken cancellationToken)
    {
        var character = await _mediator.Send(new LevelUpCharacterCommand(id), cancellationToken);
        return Ok(character);
    }

    // POST /api/characters/{id}/inventory/{itemId}
    [HttpPost("{id:guid}/inventory/{itemId:guid}")]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItem(
        Guid id, Guid itemId, CancellationToken cancellationToken)
    {
        var character = await _mediator.Send(
            new AddItemToCharacterCommand(id, itemId), cancellationToken);
        return Ok(character);
    }

    // DELETE /api/characters/{id}/inventory/{itemId}
    [HttpDelete("{id:guid}/inventory/{itemId:guid}")]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItem(
        Guid id, Guid itemId, CancellationToken cancellationToken)
    {
        var character = await _mediator.Send(
            new RemoveItemFromCharacterCommand(id, itemId), cancellationToken);
        return Ok(character);
    }

    // DELETE /api/characters/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCharacterCommand(id), cancellationToken);
        return NoContent();
    }
}

// Litet request-objekt för PUT /name – undviker att exponera hela command i body
public record UpdateCharacterNameRequest(string NewName);
