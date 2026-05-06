using RpgApi.Domain.Entities;

namespace RpgApi.Domain.Interfaces;

public interface IItemRepository : IRepository<Item>
{
    Task<IEnumerable<Item>> GetByTypeAsync(
        Domain.Enums.ItemType itemType,
        CancellationToken cancellationToken = default);
}
