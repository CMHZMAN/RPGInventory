using MediatR;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Application.Auth.Commands;

namespace RpgApi.Api.Controllers;

/// <summary>
/// Autentiseringsendpoints – dessa är PUBLIKA (ingen [Authorize]).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    /// <summary>Registrerar en ny användare och returnerar en JWT.</summary>
    /// <response code="201">Registrering lyckades – JWT returneras.</response>
    /// <response code="400">Användarnamn eller e-post redan taget.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>Loggar in en användare och returnerar en JWT.</summary>
    /// <response code="200">Inloggning lyckades – JWT returneras.</response>
    /// <response code="400">Ogiltiga inloggningsuppgifter.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}
