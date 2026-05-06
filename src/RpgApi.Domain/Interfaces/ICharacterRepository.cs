using RpgApi.Domain.Entities;

namespace RpgApi.Domain.Interfaces;

/// <summary>
/// Specifikt repository för Character med domänspecifika queries.
/// 
/// Interface Segregation Principle (ISP): vi utökar IRepository med
/// bara de metoder som är relevanta för Character, istället för att
/// stoppa allt i ett gigantiskt interface.
/// </summary>
public interface ICharacterRepository : IRepository<Character>
{
    /// <summary>
    /// Hämtar karaktär med hela inventariet (eager loading).
    /// Separata metoder för "with includes" är bättre än att alltid
    /// ladda in navigationsegenskaper – vi laddar bara vad vi behöver.
    /// </summary>
    Task<Character?> GetWithInventoryAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Character>> GetByClassAsync(
        Domain.Enums.CharacterClass characterClass,
        CancellationToken cancellationToken = default);
}
