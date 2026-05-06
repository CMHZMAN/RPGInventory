using RpgApi.Domain.Interfaces;

namespace RpgApi.Infrastructure.Services;

/// <summary>
/// BCrypt-implementation av IPasswordHasher.
/// WorkFactor 12 = 2^12 iterationer ≈ 300ms per hash på modern hårdvara.
/// Det gör brute-force extremt kostsamt utan att påverka normal användning.
/// </summary>
public class BcryptPasswordHasher : IPasswordHasher
{
    public string Hash(string plainPassword) =>
        BCrypt.Net.BCrypt.HashPassword(plainPassword, workFactor: 12);

    public bool Verify(string plainPassword, string hash) =>
        BCrypt.Net.BCrypt.Verify(plainPassword, hash);
}
