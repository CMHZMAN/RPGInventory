using RpgApi.Domain.Entities;

namespace RpgApi.Application.Common.Interfaces;

/// <summary>
/// Application-lagret definierar kontraktet – Infrastructure levererar implementationen.
/// Samma princip som IPasswordHasher: beroenden pekar alltid inåt mot kärnan.
/// </summary>
public interface IJwtService
{
    string GenerateToken(User user);
}
