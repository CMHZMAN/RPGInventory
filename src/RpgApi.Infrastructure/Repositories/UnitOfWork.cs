using RpgApi.Domain.Entities;
using RpgApi.Domain.Interfaces;
using RpgApi.Infrastructure.Persistence;

namespace RpgApi.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RpgDbContext _context;
    private ICharacterRepository?      _characters;
    private IItemRepository?           _items;
    private IRepository<CharacterItem>? _characterItems;
<<<<<<< HEAD
<<<<<<< HEAD
=======
    private IUserRepository? _users;
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
=======
    private IUserRepository?           _users;
>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API

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

<<<<<<< HEAD
<<<<<<< HEAD
=======
    public IUserRepository Users =>
        _users ??= new UserRepository(_context);

>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
=======
    public IUserRepository Users =>
        _users ??= new UserRepository(_context);

>>>>>>> Made a new branch feat: scaffold Clean Architecture foundation for RPG API
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
