using RpgApi.Domain.Common;

namespace RpgApi.Domain.Entities;

/// <summary>
/// User representerar en inloggad användare i systemet.
///
/// Lösenordet lagras ALDRIG i klartext – bara en BCrypt-hash.
/// BCrypt är ett adaptivt hashningsalgoritm som är designat för
/// att vara långsamt (work factor), vilket försvårar brute-force.
///
/// En användare äger sina karaktärer (en-till-många).
/// OwnerId på Character kan läggas till i ett framtida steg.
/// </summary>
public class User : BaseEntity
{
    public string Username { get; private set; } = string.Empty;

    /// <summary>
    /// BCrypt-hash av lösenordet.
    /// Format: $2a$11$[22 tecken salt][31 tecken hash]
    /// Exponeras ALDRIG i DTOs eller API-svar.
    /// </summary>
    public string PasswordHash { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    protected User() { }

    private User(string username, string passwordHash, string email)
    {
        Username     = username;
        PasswordHash = passwordHash;
        Email        = email;
        CreatedAt    = DateTime.UtcNow;
    }

    /// <summary>
    /// Skapar en ny användare. Tar emot en redan hashad lösenordssträng –
    /// anroparen (Application-lagret) ansvarar för att hasha via IPasswordHasher.
    /// Domänen lagrar aldrig klartext.
    /// </summary>
    public static User Create(string username, string passwordHash, string email)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Användarnamn får inte vara tomt.", nameof(username));
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Lösenordshash får inte vara tom.", nameof(passwordHash));
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Ogiltig e-postadress.", nameof(email));

        return new User(username.ToLowerInvariant(), passwordHash, email.ToLowerInvariant());
    }

    /// <summary>
    /// Verifiering sker INTE i domänentiteten längre – den vet inte om BCrypt.
    /// Anropas via IPasswordHasher.Verify() i Application-lagret.
    /// </summary>
    public bool CheckPasswordHash(string hashToCompare) =>
        PasswordHash == hashToCompare;
}
