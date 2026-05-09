using MediatR;
<<<<<<< HEAD
<<<<<<< HEAD
=======
using Microsoft.AspNetCore.Authorization;
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
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
/// CRUD-operationer för karaktärer och inventariehantering.
/// Alla endpoints kräver en giltig JWT-token.
/// </summary>
[ApiController]
[Route("api/[controller]")]
<<<<<<< HEAD
=======
/// <summary>CRUD-operationer för karaktärer och inventariehantering.</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
=======
[Authorize]
[Produces("application/json")]
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
public class CharactersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CharactersController(IMediator mediator) => _mediator = mediator;

    /// <summary>Hämtar alla karaktärer.</summary>
    /// <response code="200">En lista med alla karaktärer returneras.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CharacterDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var characters = await _mediator.Send(new GetAllCharactersQuery(), cancellationToken);
        return Ok(characters);
    }

    /// <summary>Hämtar en enskild karaktär med inventarie.</summary>
    /// <param name="id">Karaktärens unika id.</param>
    /// <response code="200">Karaktären hittades och returneras.</response>
    /// <response code="404">Ingen karaktär med det angivna id:t finns.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var character = await _mediator.Send(new GetCharacterByIdQuery(id), cancellationToken);
        return character is null ? NotFound() : Ok(character);
    }

    /// <summary>Skapar en ny karaktär.</summary>
    /// <param name="command">Namn och klass för den nya karaktären.</param>
    /// <response code="201">Karaktären skapades – returnerar den nya resursen med Location-header.</response>
    /// <response code="400">Ogiltig data i request-body.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCharacterCommand command,
        CancellationToken cancellationToken)
    {
        var character = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = character.Id }, character);
    }

    /// <summary>Byter namn på en karaktär.</summary>
    /// <param name="id">Karaktärens unika id.</param>
    /// <param name="request">Nytt namn.</param>
    /// <response code="200">Namnet uppdaterades – den uppdaterade karaktären returneras.</response>
    /// <response code="404">Ingen karaktär med det angivna id:t finns.</response>
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

    /// <summary>Höjer en karaktärs nivå med 1.</summary>
    /// <param name="id">Karaktärens unika id.</param>
    /// <response code="200">Level-up lyckades – den uppdaterade karaktären returneras.</response>
    /// <response code="404">Ingen karaktär med det angivna id:t finns.</response>
    [HttpPost("{id:guid}/levelup")]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LevelUp(Guid id, CancellationToken cancellationToken)
    {
        var character = await _mediator.Send(new LevelUpCharacterCommand(id), cancellationToken);
        return Ok(character);
    }

    /// <summary>Lägger till ett föremål i karaktärens inventarie.</summary>
    /// <param name="id">Karaktärens unika id.</param>
    /// <param name="itemId">Föremålets unika id.</param>
    /// <response code="200">Föremålet lades till – den uppdaterade karaktären returneras.</response>
    /// <response code="400">Föremålet finns redan i inventariet eller annan domänregel bröts.</response>
    /// <response code="404">Karaktären eller föremålet hittades inte.</response>
    [HttpPost("{id:guid}/inventory/{itemId:guid}")]
    [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem(
        Guid id, Guid itemId, CancellationToken cancellationToken)
    {
        var character = await _mediator.Send(
            new AddItemToCharacterCommand(id, itemId), cancellationToken);
        return Ok(character);
    }

    /// <summary>Tar bort ett föremål från karaktärens inventarie.</summary>
    /// <param name="id">Karaktärens unika id.</param>
    /// <param name="itemId">Inventariepostens unika id.</param>
    /// <response code="200">Föremålet togs bort – den uppdaterade karaktären returneras.</response>
    /// <response code="404">Karaktären eller inventarieposten hittades inte.</response>
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

    /// <summary>Tar bort en karaktär permanent.</summary>
    /// <param name="id">Karaktärens unika id.</param>
    /// <response code="204">Karaktären togs bort.</response>
    /// <response code="404">Ingen karaktär med det angivna id:t finns.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCharacterCommand(id), cancellationToken);
        return NoContent();
    }
}

/// <summary>Request-body för att byta namn på en karaktär.</summary>
/// <param name="NewName">Det nya namnet.</param>
public record UpdateCharacterNameRequest(string NewName);
