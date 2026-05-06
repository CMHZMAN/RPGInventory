using MediatR;
using Microsoft.Extensions.Configuration;
using RpgApi.Application.Common.Exceptions;
using RpgApi.Application.Common.Interfaces;
using RpgApi.Domain.Entities;
using RpgApi.Domain.Interfaces;

namespace RpgApi.Application.Auth.Commands;

/// <summary>
/// Hanterar registrering av ny användare.
/// Flöde: Validera unikhet → Skapa User (domänen hashar lösenordet) → Spara → Returnera token.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUnitOfWork     _uow;
    private readonly IJwtService     _jwt;
    private readonly int             _expiryMinutes;
    private readonly IPasswordHasher _hasher;

    public RegisterCommandHandler(IUnitOfWork uow, IJwtService jwt,
        IConfiguration config, IPasswordHasher hasher)
    {
        _uow           = uow;
        _jwt           = jwt;
        _hasher        = hasher;
        _expiryMinutes = int.TryParse(config["Jwt:ExpiryMinutes"], out var m) ? m : 60;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
            throw new DomainException("Lösenordet måste vara minst 6 tecken.");

        if (await _uow.Users.UsernameExistsAsync(request.Username, ct))
            throw new DomainException($"Användarnamnet '{request.Username}' är redan taget.");

        if (await _uow.Users.EmailExistsAsync(request.Email, ct))
            throw new DomainException($"E-postadressen är redan registrerad.");

        // Hasha lösenordet i Application-lagret via interfacet
        var hash = _hasher.Hash(request.Password);
        var user = User.Create(request.Username, hash, request.Email);

        await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        var token = _jwt.GenerateToken(user);
        return new AuthResponse(token, user.Username, DateTime.UtcNow.AddMinutes(_expiryMinutes));
    }
}

/// <summary>
/// Hanterar inloggning.
/// Vi använder samma generiska felmeddelande oavsett om användaren
/// inte finns ELLER lösenordet är fel – det förhindrar "username enumeration".
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUnitOfWork     _uow;
    private readonly IJwtService     _jwt;
    private readonly int             _expiryMinutes;
    private readonly IPasswordHasher _hasher;

    public LoginCommandHandler(IUnitOfWork uow, IJwtService jwt,
        IConfiguration config, IPasswordHasher hasher)
    {
        _uow           = uow;
        _jwt           = jwt;
        _hasher        = hasher;
        _expiryMinutes = int.TryParse(config["Jwt:ExpiryMinutes"], out var m) ? m : 60;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByUsernameAsync(request.Username, ct);

        // Samma felmeddelande för "hittades inte" och "fel lösenord"
        // Det förhindrar "username enumeration"-attacker
        if (user is null || !_hasher.Verify(request.Password, user.PasswordHash))
            throw new DomainException("Ogiltigt användarnamn eller lösenord.");

        var token = _jwt.GenerateToken(user);
        return new AuthResponse(token, user.Username, DateTime.UtcNow.AddMinutes(_expiryMinutes));
    }
}
