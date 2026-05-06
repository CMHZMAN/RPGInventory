using RpgApi.Domain.Interfaces;
using RpgApi.Infrastructure.Persistence;

namespace RpgApi.Infrastructure.Repositories;

/// <summary>
/// Unit of Work-implementationen wrappar DbContext.
/// 
/// Alla repositories delar SAMMA DbContext-instans.
/// Det är nyckeln: EF Cores Change Tracker håller koll på alla
/// ändringar oavsett vilket repository som gjorde dem.
/// SaveChangesAsync sparar ALLT i en enda transaktion.
/// 
/// Lazy initialization med '??=' (null-coalescing assignment):
/// repository skapas inte förrän det faktiskt används.
/// Sparar minne om t.ex. Items aldrig används i en given request.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly RpgDbContext _context;
    private ICharacterRepository? _characters;
    private IItemRepository? _items;

    public UnitOfWork(RpgDbContext context)
    {
        _context = context;
    }

    // Lazy initialization: '??=' betyder "om null, tilldela och returnera"
    public ICharacterRepository Characters =>
        _characters ??= new CharacterRepository(_context);

    public IItemRepository Items =>
        _items ??= new ItemRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
