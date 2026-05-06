namespace RpgApi.Domain.Common;

/// <summary>
/// Abstrakt basklass som alla domänentiteter ärver från.
/// 
/// Varför abstrakt? Vi vill aldrig skapa ett "BaseEntity" direkt –
/// det är bara en mall. 'abstract' tvingar kompilatorn att hindra det.
/// 
/// Varför Guid som Id? Till skillnad från int (som genereras av databasen)
/// kan vi skapa ett Guid redan i applikationskoden, INNAN vi träffar databasen.
/// Det gör testning enklare och fungerar bra i distribuerade system.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    // protected set = bara klassen själv och subklasser kan sätta värdet.
    // Det skyddar mot att utomstående kod sätter Id:t direkt.
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    // Denna konstruktor används av EF Core vid inläsning från databasen.
    // EF Core behöver kunna återskapa objekt utan att generera nytt Guid.
    protected BaseEntity(Guid id)
    {
        Id = id;
    }
}
