using Microsoft.EntityFrameworkCore;
using RpgApi.Domain.Entities;
using RpgApi.Domain.Enums;
using RpgApi.Domain.Interfaces;
using RpgApi.Infrastructure.Persistence;

namespace RpgApi.Infrastructure.Repositories;

public class ItemRepository : Repository<Item>, IItemRepository
{
    public ItemRepository(RpgDbContext context) : base(context) { }

    public async Task<IEnumerable<Item>> GetByTypeAsync(
        ItemType itemType,
        CancellationToken cancellationToken = default)
    {
        return await _context.Items
            .AsNoTracking()
            .Where(i => i.Type == itemType)
            .OrderBy(i => i.Name)
            .ToListAsync(cancellationToken);
    }
}
