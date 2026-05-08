using MediatR;

namespace RpgApi.Application.Auth.Commands;

/// <summary>
/// Register: skapar ett nytt konto.
/// Login: autentiserar och returnerar en JWT.
/// Båda returnerar samma AuthResponse – klienten loggas in direkt vid registrering.
/// </summary>
public record RegisterCommand(
    string Username,
    string Password,
    string Email
) : IRequest<AuthResponse>;

public record LoginCommand(
    string Username,
    string Password
) : IRequest<AuthResponse>;

/// <summary>
/// Svar vid lyckad autentisering.
/// Token är den JWT klienten ska spara och skicka med framtida requests.
/// ExpiresAt hjälper frontend att förnya token i tid.
/// </summary>
public record AuthResponse(
    string Token,
    string Username,
    DateTime ExpiresAt
);
