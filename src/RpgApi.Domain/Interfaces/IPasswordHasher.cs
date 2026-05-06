namespace RpgApi.Domain.Interfaces;

/// <summary>
/// Abstraherar lösenordshashning ur domänen.
/// Domain definierar KONTRAKTET – Infrastructure väljer BCrypt, Argon2 etc.
/// Det här är Dependency Inversion Principle i praktiken:
/// Domain beror på ett interface, inte på ett specifikt hashbibliotek.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string plainPassword);
    bool Verify(string plainPassword, string hash);
}
