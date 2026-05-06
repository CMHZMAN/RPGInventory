using Microsoft.EntityFrameworkCore;
using RpgApi.Domain.Entities;
using RpgApi.Domain.Enums;
using RpgApi.Domain.Interfaces;
using RpgApi.Infrastructure.Persistence;

namespace RpgApi.Infrastructure.Repositories;

/// <summary>
/// Konkret implementation av ICharacterRepository.
/// Ärver från generiska Repository<Character> och lägger till
/// Character-specifika queries.
/// 
/// 'override' på GetByIdAsync visar att vi VÄLJER att definiera
/// om basklassens beteende för detta specifika fall.
/// </summary>
public class CharacterRepository : Repository<Character>, ICharacterRepository
{
    public CharacterRepository(RpgDbContext context) : base(context) { }

    /// <summary>
    /// Override: vi vill ladda inventariet (Inventory) automatiskt
    /// via Include/ThenInclude när vi hämtar en enskild karaktär.
    /// Basklassens FindAsync laddar inte navigationsegenskaper.
    /// </summary>
    public override async Task<Character?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Characters
            .Include(c => c.Inventory)
                .ThenInclude(ci => ci.Item)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Character?> GetWithInventoryAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        // Samma som ovan, men tydliggör intentionen med metodnamnet
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Character>> GetByClassAsync(
        CharacterClass characterClass,
        CancellationToken cancellationToken = default)
    {
        return await _context.Characters
            .AsNoTracking()
            .Where(c => c.Class == characterClass)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }
}
