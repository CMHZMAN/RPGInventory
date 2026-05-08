using Microsoft.EntityFrameworkCore;
using RpgApi.Domain.Entities;
using RpgApi.Domain.Interfaces;
using RpgApi.Infrastructure.Persistence;

namespace RpgApi.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(RpgDbContext context) : base(context) { }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default) =>
        await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username.ToLowerInvariant(), ct);

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken ct = default) =>
        await _context.Users
            .AnyAsync(u => u.Username == username.ToLowerInvariant(), ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default) =>
        await _context.Users
            .AnyAsync(u => u.Email == email.ToLowerInvariant(), ct);
}
