using MediatR;
<<<<<<< HEAD
=======
using Microsoft.AspNetCore.Authorization;
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
using Microsoft.AspNetCore.Mvc;
using RpgApi.Application.Items.Commands;
using RpgApi.Application.Items.DTOs;
using RpgApi.Application.Items.Queries;

namespace RpgApi.Api.Controllers;

<<<<<<< HEAD
[ApiController]
[Route("api/[controller]")]
=======
/// <summary>CRUD-operationer för föremål.</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetAllItemsQuery(), cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await _mediator.Send(new GetItemByIdQuery(id), cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateItemCommand command,
        CancellationToken cancellationToken)
    {
        var item = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateItemCommand(id, request.Name, request.Description,
            request.StrengthBonus, request.IntelligenceBonus,
            request.AgilityBonus, request.DefenseBonus);

        var item = await _mediator.Send(command, cancellationToken);
        return Ok(item);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteItemCommand(id), cancellationToken);
        return NoContent();
    }
}

public record UpdateItemRequest(
    string Name,
    string Description,
    int StrengthBonus,
    int IntelligenceBonus,
    int AgilityBonus,
    int DefenseBonus
);
