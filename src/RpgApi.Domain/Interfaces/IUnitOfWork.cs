namespace RpgApi.Domain.Interfaces;

/// <summary>Unit of Work koordinerar transaktioner over flera repositories.</summary>
public interface IUnitOfWork
{
    ICharacterRepository Characters { get; }
    IItemRepository Items { get; }
    IUserRepository Users { get; }
    IRepository<Domain.Entities.CharacterItem> CharacterItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
