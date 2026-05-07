import { useParams, useNavigate } from 'react-router-dom';
import { useCharacter, useItems, useAddItem, useRemoveItem, useLevelUp } from '../hooks/useRpgApi';
import { ClassBadge, ErrorMessage, HealthBar, Spinner, StatBox } from '../components/ui/Common';
import type { Item } from '../types/api.types';

/**
 * Detaljsida för en enskild karaktär.
 * Visar all info + inventariet + föremål att lägga till.
 */
export function CharacterDetailPage() {
  const { id = '' }    = useParams<{ id: string }>();
  const navigate       = useNavigate();
  const { data: character, isLoading, error } = useCharacter(id);
  const { data: allItems = [] }               = useItems();
  const addItem    = useAddItem();
  const removeItem = useRemoveItem();
  const levelUp    = useLevelUp();

  if (isLoading) return <Spinner />;
  if (error || !character) return <ErrorMessage message={error?.message ?? 'Karaktär hittades inte'} />;

  // Föremål som INTE redan finns i inventariet
  const inventoryItemIds = new Set(character.inventory.map((ci) => ci.itemId));
  const availableItems   = allItems.filter((i) => !inventoryItemIds.has(i.id));

  return (
    <div className="container">
      {/* ── Back + Title ─────────────────────────────── */}
      <div className="page-header">
        <button className="btn btn-ghost btn-sm" onClick={() => navigate('/')}>← Tillbaka</button>
        <div className="flex-between mt-2">
          <div>
            <h1>{character.name}</h1>
            <div className="flex gap-1 mt-1">
              <ClassBadge className={character.class} />
              <span className="text-muted" style={{ fontSize: '.9rem' }}>Nivå {character.level}</span>
            </div>
          </div>
          <button
            className="btn btn-success"
            onClick={() => levelUp.mutate(character.id)}
            disabled={levelUp.isPending}
          >
            ⬆ Level up
          </button>
        </div>
      </div>

      {/* ── Hälsa ────────────────────────────────────── */}
      <div className="card mt-2">
        <HealthBar current={character.currentHealth} max={character.maxHealth} />
      </div>

      {/* ── Stats ────────────────────────────────────── */}
      <div className="card mt-2">
        <h2 className="text-gold" style={{ marginBottom: '.8rem', fontSize: '1rem' }}>📊 Statistik</h2>
        <div className="stats-grid">
          <StatBox label="STR" value={character.strength}     />
          <StatBox label="INT" value={character.intelligence}  />
          <StatBox label="AGI" value={character.agility}       />
          <StatBox label="DEF" value={character.defense}       />
          <StatBox label="MaxHP" value={character.maxHealth}   />
        </div>
      </div>

      {/* ── Inventarie ───────────────────────────────── */}
      <div className="card mt-2">
        <h2 className="text-gold" style={{ marginBottom: '.8rem', fontSize: '1rem' }}>
          🎒 Inventarie ({character.inventory.length}/20)
        </h2>
        {character.inventory.length === 0 ? (
          <p className="text-muted">Inventariet är tomt.</p>
        ) : (
          <div className="item-grid">
            {character.inventory.map((ci) => (
              <div key={ci.characterItemId} className="card" style={{ padding: '.8rem' }}>
                <div className="flex-between">
                  <strong style={{ fontSize: '.9rem' }}>{ci.name}</strong>
                  <span style={{ fontSize: '.75rem', color: 'var(--text-muted)' }}>{ci.type}</span>
                </div>
                <p style={{ fontSize: '.78rem', color: 'var(--text-muted)', margin: '.3rem 0' }}>
                  {ci.description}
                </p>
                <BonusList item={ci} />
                <button
                  className="btn btn-danger btn-sm mt-1"
                  style={{ width: '100%' }}
                  onClick={() => removeItem.mutate({ characterId: character.id, itemId: ci.itemId })}
                  disabled={removeItem.isPending}
                >
                  Ta bort
                </button>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* ── Lägg till föremål ────────────────────────── */}
      {availableItems.length > 0 && (
        <div className="card mt-2">
          <h2 className="text-gold" style={{ marginBottom: '.8rem', fontSize: '1rem' }}>
            ➕ Lägg till föremål
          </h2>
          <div className="item-grid">
            {availableItems.map((item) => (
              <div key={item.id} className="card" style={{ padding: '.8rem' }}>
                <div className="flex-between">
                  <strong style={{ fontSize: '.9rem' }}>{item.name}</strong>
                  <span style={{ fontSize: '.75rem', color: 'var(--text-muted)' }}>{item.type}</span>
                </div>
                <p style={{ fontSize: '.78rem', color: 'var(--text-muted)', margin: '.3rem 0' }}>
                  {item.description}
                </p>
                <BonusList item={item} />
                <button
                  className="btn btn-primary btn-sm mt-1"
                  style={{ width: '100%' }}
                  onClick={() => addItem.mutate({ characterId: character.id, itemId: item.id })}
                  disabled={addItem.isPending}
                >
                  Lägg till
                </button>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}

/** Visar bonusar för ett föremål – renderar bara de som är > 0. */
function BonusList({ item }: { item: Pick<Item, 'strengthBonus' | 'intelligenceBonus' | 'agilityBonus' | 'defenseBonus'> }) {
  const bonuses = [
    { label: '⚔ STR', value: item.strengthBonus },
    { label: '🔮 INT', value: item.intelligenceBonus },
    { label: '💨 AGI', value: item.agilityBonus },
    { label: '🛡 DEF', value: item.defenseBonus },
  ].filter((b) => b.value > 0);

  if (bonuses.length === 0) return null;

  return (
    <div className="flex gap-1" style={{ flexWrap: 'wrap', marginTop: '.4rem' }}>
      {bonuses.map((b) => (
        <span
          key={b.label}
          style={{ fontSize: '.72rem', background: 'var(--surface-2)', padding: '.15rem .5rem', borderRadius: '99px', color: 'var(--gold)' }}
        >
          {b.label} +{b.value}
        </span>
      ))}
    </div>
  );
}
