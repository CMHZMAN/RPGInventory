using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Application.Characters.Commands;
using RpgApi.Application.Characters.DTOs;
using RpgApi.Application.Characters.Queries;

namespace RpgApi.Api.Controllers;

/// <summary>CRUD-operationer for karaktarer.</summary>
[ApiController][Route("api/[controller]")][Authorize][Produces("application/json")]
public class CharactersController : ControllerBase
{
    private readonly IMediator _mediator;
    public CharactersController(IMediator mediator) => _mediator = mediator;

    /// <summary>Hamtar alla karaktarer.</summary><response code="200">Lista returneras.</response>
    [HttpGet][ProducesResponseType(typeof(IEnumerable<CharacterDto>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct) => Ok(await _mediator.Send(new GetAllCharactersQuery(), ct));

    /// <summary>Hamtar en karaktar.</summary>
    /// <param name="id">Karaktarens id.</param>
    /// <response code="200">Hittades.</response><response code="404">Hittades ej.</response>
    [HttpGet("{id:guid}")][ProducesResponseType(typeof(CharacterDto), 200)][ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    { var c = await _mediator.Send(new GetCharacterByIdQuery(id), ct); return c is null ? NotFound() : Ok(c); }

    /// <summary>Skapar en karaktar.</summary>
    /// <param name="command">Namn och klass.</param>
    /// <response code="201">Skapad.</response><response code="400">Ogiltig data.</response>
    [HttpPost][ProducesResponseType(typeof(CharacterDto), 201)][ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateCharacterCommand command, CancellationToken ct)
    { var c = await _mediator.Send(command, ct); return CreatedAtAction(nameof(GetById), new { id = c.Id }, c); }

    /// <summary>Byter namn.</summary><param name="id">Id.</param><param name="request">Nytt namn.</param>
    /// <response code="200">Uppdaterat.</response><response code="404">Hittades ej.</response>
    [HttpPut("{id:guid}/name")][ProducesResponseType(typeof(CharacterDto), 200)][ProducesResponseType(404)]
    public async Task<IActionResult> UpdateName(Guid id, [FromBody] UpdateCharacterNameRequest request, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateCharacterNameCommand(id, request.NewName), ct));

    /// <summary>Level up.</summary><param name="id">Id.</param>
    /// <response code="200">Klar.</response><response code="404">Hittades ej.</response>
    [HttpPost("{id:guid}/levelup")][ProducesResponseType(typeof(CharacterDto), 200)][ProducesResponseType(404)]
    public async Task<IActionResult> LevelUp(Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new LevelUpCharacterCommand(id), ct));

    /// <summary>Lagg till foremal.</summary><param name="id">Karaktar-id.</param><param name="itemId">Foremal-id.</param>
    /// <response code="200">Lagt till.</response><response code="400">Redan i inventariet.</response><response code="404">Hittades ej.</response>
    [HttpPost("{id:guid}/inventory/{itemId:guid}")][ProducesResponseType(typeof(CharacterDto), 200)][ProducesResponseType(400)][ProducesResponseType(404)]
    public async Task<IActionResult> AddItem(Guid id, Guid itemId, CancellationToken ct)
        => Ok(await _mediator.Send(new AddItemToCharacterCommand(id, itemId), ct));

    /// <summary>Ta bort foremal.</summary><param name="id">Karaktar-id.</param><param name="itemId">Inventarie-id.</param>
    /// <response code="200">Borttaget.</response><response code="404">Hittades ej.</response>
    [HttpDelete("{id:guid}/inventory/{itemId:guid}")][ProducesResponseType(typeof(CharacterDto), 200)][ProducesResponseType(404)]
    public async Task<IActionResult> RemoveItem(Guid id, Guid itemId, CancellationToken ct)
        => Ok(await _mediator.Send(new RemoveItemFromCharacterCommand(id, itemId), ct));

    /// <summary>Tar bort karaktar.</summary><param name="id">Id.</param>
    /// <response code="204">Borttagen.</response><response code="404">Hittades ej.</response>
    [HttpDelete("{id:guid}")][ProducesResponseType(204)][ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    { await _mediator.Send(new DeleteCharacterCommand(id), ct); return NoContent(); }
}

public record UpdateCharacterNameRequest(string NewName);