using RpgApi.Domain.Entities;
using RpgApi.Domain.Interfaces;
using RpgApi.Infrastructure.Persistence;

namespace RpgApi.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RpgDbContext _context;
    private ICharacterRepository? _characters;
    private IItemRepository? _items;
    private IRepository<CharacterItem>? _characterItems;

    public UnitOfWork(RpgDbContext context)
    {
        _context = context;
    }

    public ICharacterRepository Characters =>
        _characters ??= new CharacterRepository(_context);

    public IItemRepository Items =>
        _items ??= new ItemRepository(_context);

    public IRepository<CharacterItem> CharacterItems =>
        _characterItems ??= new Repository<CharacterItem>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
