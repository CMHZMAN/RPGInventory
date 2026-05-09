using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RpgApi.Application.Common.Interfaces;
using RpgApi.Domain.Entities;

namespace RpgApi.Infrastructure.Services;

/// <summary>
/// Genererar och validerar JWT-tokens.
///
/// JWT (JSON Web Token) består av tre delar separerade av punkter:
///   Header.Payload.Signature
///
/// Header:  algoritm + typ (HS256, JWT)
/// Payload: claims – data om användaren (id, namn, roller)
/// Signature: HMAC-SHA256(header + payload, hemlig nyckel)
///
/// Varje HTTP-request skickar token i Authorization-headern:
///   Authorization: Bearer eyJhbGci...
///
/// API:et verifierar signaturen – om den är giltig kan det
/// lita på claims utan att slå upp databasen varje gång.
/// </summary>
public class JwtService : IJwtService
{
<<<<<<< HEAD
    private readonly string            _issuer;
    private readonly string            _audience;
    private readonly int               _expiryMinutes;
    private readonly SigningCredentials _signingCredentials;
=======
    private readonly string                 _issuer;
    private readonly string                 _audience;
    private readonly int                    _expiryMinutes;
    private readonly SigningCredentials      _signingCredentials;
    // JwtSecurityTokenHandler är thread-safe och avsedd att återanvändas.
    // En ny instans per anrop kostar ~25 % av metodens CPU-budget.
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API

    public JwtService(IConfiguration config)
    {
        var section    = config.GetSection("Jwt");
        var secret     = section["Secret"] ?? throw new InvalidOperationException("Jwt:Secret saknas i konfigurationen.");
        _issuer        = section["Issuer"]   ?? "RpgApi";
        _audience      = section["Audience"] ?? "RpgClient";
        _expiryMinutes = int.TryParse(section["ExpiryMinutes"], out var m) ? m : 60;

        // SymmetricSecurityKey och SigningCredentials är dyra att skapa och oföränderliga.
        // Vi skapar dem en gång i konstruktorn och återanvänder dem för varje token.
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    /// <summary>
    /// Skapar en signerad JWT för en användare.
    /// Claims är "påståenden" om användaren som lagras i token.
    /// Vi inkluderar: Id, Username.
    /// </summary>
    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            // Sub (Subject) = standard JWT-claim för användarens unika id
            new Claim(JwtRegisteredClaimNames.Sub,        user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            // Jti (JWT ID) = unikt id för just denna token (möjliggör token-blacklisting)
            new Claim(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString()),
        };

        var now   = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer:             _issuer,
            audience:           _audience,
            claims:             claims,
            notBefore:          now,
            expires:            now.AddMinutes(_expiryMinutes),
            signingCredentials: _signingCredentials   // Återanvänd cached credentials
        );

<<<<<<< HEAD
        return new JwtSecurityTokenHandler().WriteToken(token);
=======
        return _tokenHandler.WriteToken(token);
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
    }
}
