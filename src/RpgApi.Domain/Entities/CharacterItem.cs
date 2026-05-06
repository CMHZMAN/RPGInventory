using RpgApi.Domain.Common;

namespace RpgApi.Domain.Entities;

/// <summary>
/// CharacterItem är en kopplingsentiitet (junction entity) som
/// representerar att en karaktär äger ett specifikt föremål.
/// 
/// I en många-till-många-relation (Character ↔ Item) behöver vi
/// en mellantabell. EF Core kan hantera detta implicit, men en
/// explicit junction entity ger oss möjlighet att lägga till
/// extra data (t.ex. Quantity, IsEquipped, AcquiredAt).
/// </summary>
public class CharacterItem : BaseEntity
{
    public Guid CharacterId { get; private set; }
    public Guid ItemId { get; private set; }
    public bool IsEquipped { get; private set; }
    public DateTime AcquiredAt { get; private set; }

    // Navigationsegenskaper – EF Core fyller dessa automatiskt
    // när vi Include():ar dem i en query.
    public Character? Character { get; private set; }
    public Item? Item { get; private set; }

    protected CharacterItem() { }

    private CharacterItem(Guid characterId, Guid itemId)
    {
        CharacterId = characterId;
        ItemId = itemId;
        IsEquipped = false;
        AcquiredAt = DateTime.UtcNow;
    }

    public static CharacterItem Create(Guid characterId, Guid itemId)
    {
        if (characterId == Guid.Empty)
            throw new ArgumentException("CharacterId får inte vara tomt.", nameof(characterId));
        if (itemId == Guid.Empty)
            throw new ArgumentException("ItemId får inte vara tomt.", nameof(itemId));

        return new CharacterItem(characterId, itemId);
    }

    /// <summary>
    /// Ekviperingslogik – affärsregel: man kan inte ekipera något man inte bär.
    /// </summary>
    public void Equip()
    {
        if (IsEquipped)
            throw new InvalidOperationException("Föremålet är redan ekiperat.");
        IsEquipped = true;
    }

    public void Unequip() => IsEquipped = false;
}
