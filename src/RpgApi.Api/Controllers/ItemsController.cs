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
using RpgApi.Application.Items.Commands;
using RpgApi.Application.Items.DTOs;
using RpgApi.Application.Items.Queries;

namespace RpgApi.Api.Controllers;

<<<<<<< HEAD
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
=======
/// <summary>
/// CRUD-operationer för föremål i spelet.
/// Alla endpoints kräver en giltig JWT-token.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Hämtar alla föremål.</summary>
    /// <response code="200">En lista med alla föremål returneras.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetAllItemsQuery(), cancellationToken);
        return Ok(items);
    }

    /// <summary>Hämtar ett enskilt föremål.</summary>
    /// <param name="id">Föremålets unika id.</param>
    /// <response code="200">Föremålet hittades och returneras.</response>
    /// <response code="404">Inget föremål med det angivna id:t finns.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await _mediator.Send(new GetItemByIdQuery(id), cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    /// <summary>Skapar ett nytt föremål.</summary>
    /// <param name="command">Data för det nya föremålet.</param>
    /// <response code="201">Föremålet skapades – returnerar den nya resursen med Location-header.</response>
    /// <response code="400">Ogiltig data i request-body.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateItemCommand command,
        CancellationToken cancellationToken)
    {
        var item = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    /// <summary>Uppdaterar ett befintligt föremål.</summary>
    /// <param name="id">Föremålets unika id.</param>
    /// <param name="request">De nya värdena för föremålet.</param>
    /// <response code="200">Föremålet uppdaterades och returneras.</response>
    /// <response code="404">Inget föremål med det angivna id:t finns.</response>
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

    /// <summary>Tar bort ett föremål permanent.</summary>
    /// <param name="id">Föremålets unika id.</param>
    /// <response code="204">Föremålet togs bort.</response>
    /// <response code="404">Inget föremål med det angivna id:t finns.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteItemCommand(id), cancellationToken);
        return NoContent();
    }
}

/// <summary>Request-body för att uppdatera ett föremål.</summary>
public record UpdateItemRequest(
    string Name,
    string Description,
    int StrengthBonus,
    int IntelligenceBonus,
    int AgilityBonus,
    int DefenseBonus
);