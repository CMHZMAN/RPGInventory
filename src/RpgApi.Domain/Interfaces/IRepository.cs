namespace RpgApi.Domain.Interfaces;

/// <summary>
/// Generiskt repository-interface med gemensamma CRUD-operationer.
/// 
/// Dependency Inversion Principle (DIP) – det 5:e SOLID-principen:
/// Högnivå-moduler (Application) ska inte bero på lågnivå-moduler (Infrastructure).
/// Båda ska bero på abstraktioner (detta interface).
/// 
/// Gränssnittets plats i Domain är MEDVETEN:
/// - Domain definierar VAD som behövs (kontraktet)
/// - Infrastructure implementerar HUR det görs (EF Core, SQL)
/// 
/// CancellationToken i alla async-metoder = best practice för att
/// kunna avbryta long-running operations (t.ex. HTTP request cancelled).
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
