namespace RpgApi.Domain.Interfaces;

/// <summary>
/// Unit of Work-mönstret koordinerar transaktioner över flera repositories.
/// 
/// Problem utan UoW: om vi sparar Character via ICharacterRepository och
/// Item via IItemRepository i separata anrop – vad händer om det kraschar mitt i?
/// Vi kan då ha en inkonsistent databas (ett objekt sparat, ett inte).
/// 
/// Med UoW: alla ändringar samlas ihop och sparas i EN transaktion.
/// Antingen lyckas allt, eller rullas allt tillbaka.
/// 
/// I EF Core implementeras detta naturligt av DbContext – den är i sig
/// ett Unit of Work. Vi wrappar det bara i ett interface för testbarhet.
/// </summary>
public interface IUnitOfWork
{
    ICharacterRepository Characters { get; }
    IItemRepository Items { get; }

    /// <summary>
    /// Explicit spårning av CharacterItem behövs eftersom EF Core's change tracker
    /// inte alltid detekterar additions till privata backing fields automatiskt.
    /// Explicit AddAsync säkerställer korrekt EntityState.Added → INSERT.
    /// </summary>
    IRepository<Domain.Entities.CharacterItem> CharacterItems { get; }

    /// <summary>
    /// Sparar alla ändringar som samlats sedan senaste anropet.
    /// Returnerar antal påverkade rader.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
