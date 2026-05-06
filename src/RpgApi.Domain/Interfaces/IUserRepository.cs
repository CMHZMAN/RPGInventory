using RpgApi.Domain.Entities;

namespace RpgApi.Domain.Interfaces;

/// <summary>
/// Repository-interface för User.
/// Separata metoder för vanliga uppslagningar håller queries optimerade –
/// vi hämtar aldrig mer data än nödvändigt.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
}
