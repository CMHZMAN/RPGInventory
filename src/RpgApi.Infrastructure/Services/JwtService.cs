using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RpgApi.Application.Common.Interfaces;
using RpgApi.Domain.Entities;

namespace RpgApi.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly string                  _issuer;
    private readonly string                  _audience;
    private readonly int                     _expiryMinutes;
    private readonly SigningCredentials       _signingCredentials;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public JwtService(IConfiguration config)
    {
        var section    = config.GetSection("Jwt");
        var secret     = section["Secret"] ?? throw new InvalidOperationException("Jwt:Secret saknas.");
        _issuer        = section["Issuer"]   ?? "RpgApi";
        _audience      = section["Audience"] ?? "RpgClient";
        _expiryMinutes = int.TryParse(section["ExpiryMinutes"], out var m) ? m : 60;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,        user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString()),
        };
        var now   = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer:             _issuer,
            audience:           _audience,
            claims:             claims,
            notBefore:          now,
            expires:            now.AddMinutes(_expiryMinutes),
            signingCredentials: _signingCredentials);
        return _tokenHandler.WriteToken(token);
    }
}
