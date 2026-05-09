using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Application.Items.Commands;
using RpgApi.Application.Items.DTOs;
using RpgApi.Application.Items.Queries;

namespace RpgApi.Api.Controllers;

/// <summary>CRUD-operationer for foremal.</summary>
[ApiController][Route("api/[controller]")][Authorize][Produces("application/json")]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ItemsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Hamtar alla foremal.</summary><response code="200">Lista returneras.</response>
    [HttpGet][ProducesResponseType(typeof(IEnumerable<ItemDto>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct) => Ok(await _mediator.Send(new GetAllItemsQuery(), ct));

    /// <summary>Hamtar ett foremal.</summary><param name="id">Id.</param>
    /// <response code="200">Hittades.</response><response code="404">Hittades ej.</response>
    [HttpGet("{id:guid}")][ProducesResponseType(typeof(ItemDto), 200)][ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    { var item = await _mediator.Send(new GetItemByIdQuery(id), ct); return item is null ? NotFound() : Ok(item); }

    /// <summary>Skapar ett foremal.</summary><param name="command">Data.</param>
    /// <response code="201">Skapat.</response><response code="400">Ogiltig data.</response>
    [HttpPost][ProducesResponseType(typeof(ItemDto), 201)][ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateItemCommand command, CancellationToken ct)
    { var item = await _mediator.Send(command, ct); return CreatedAtAction(nameof(GetById), new { id = item.Id }, item); }

    /// <summary>Uppdaterar ett foremal.</summary><param name="id">Id.</param><param name="request">Nya varden.</param>
    /// <response code="200">Uppdaterat.</response><response code="404">Hittades ej.</response>
    [HttpPut("{id:guid}")][ProducesResponseType(typeof(ItemDto), 200)][ProducesResponseType(404)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateItemRequest request, CancellationToken ct)
    {
        var cmd = new UpdateItemCommand(id, request.Name, request.Description,
            request.StrengthBonus, request.IntelligenceBonus, request.AgilityBonus, request.DefenseBonus);
        return Ok(await _mediator.Send(cmd, ct));
    }

    /// <summary>Tar bort ett foremal.</summary><param name="id">Id.</param>
    /// <response code="204">Borttaget.</response><response code="404">Hittades ej.</response>
    [HttpDelete("{id:guid}")][ProducesResponseType(204)][ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    { await _mediator.Send(new DeleteItemCommand(id), ct); return NoContent(); }
}

public record UpdateItemRequest(string Name, string Description, int StrengthBonus, int IntelligenceBonus, int AgilityBonus, int DefenseBonus);