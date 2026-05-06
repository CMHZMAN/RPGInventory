using Microsoft.EntityFrameworkCore;
using RpgApi.Domain.Interfaces;
using RpgApi.Infrastructure.Persistence;

namespace RpgApi.Infrastructure.Repositories;

/// <summary>
/// Generisk bas-implementation av IRepository<T>.
/// 
/// Generic Repository Pattern: istället för att skriva samma
/// GetById, Add, Delete etc. i varje repository, lägger vi det
/// en gång i en generisk basklass.
/// 
/// T måste vara en klass (where T : class) – samma constraint
/// som i interface-definitionen. EF Core kräver detta för DbSet<T>.
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    // protected = subklasser (CharacterRepository) kan komma åt dessa
    protected readonly RpgDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(RpgDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        // FindAsync söker först i EF Cores Identity Map (minnet),
        // sedan i databasen. Mer effektivt än FirstOrDefaultAsync
        // när man söker på primary key.
        return await _dbSet.FindAsync([id], cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        // AsNoTracking: EF Core spårar inte dessa entiteter.
        // Snabbare för read-only queries – vi tänker inte uppdatera dem.
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(T entity,
        CancellationToken cancellationToken = default)
    {
        // AddAsync markerar entiteten som "Added" i EF Cores Change Tracker.
        // Den faktiska SQL INSERT sker inte förrän SaveChangesAsync anropas.
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(T entity)
    {
        // Update markerar entiteten som "Modified" i Change Tracker.
        // Synkront eftersom det bara uppdaterar in-memory state.
        _dbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        // Remove markerar som "Deleted". SQL DELETE sker vid SaveChanges.
        _dbSet.Remove(entity);
    }

    public virtual async Task<bool> ExistsAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        // AnyAsync är mer effektivt än GetById + null-check.
        // Genererar "SELECT CASE WHEN EXISTS(...) THEN 1 ELSE 0 END"
        // istället för att hämta hela raden.
        return await _dbSet.AnyAsync(
            e => EF.Property<Guid>(e, "Id") == id,
            cancellationToken);
    }
}
