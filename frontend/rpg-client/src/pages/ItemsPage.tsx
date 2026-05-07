import { useItems } from '../hooks/useRpgApi';
import { ErrorMessage, Spinner } from '../components/ui/Common';

const TYPE_EMOJI: Record<string, string> = {
  Weapon: '⚔️', Armor: '🛡️', Potion: '🧪', Accessory: '💍',
};

/** Visar alla föremål i en grid med bonusinfo. */
export function ItemsPage() {
  const { data: items, isLoading, error } = useItems();

  if (isLoading) return <Spinner />;
  if (error)     return <ErrorMessage message={error.message} />;

  return (
    <div className="container">
      <div className="page-header">
        <h1>🗡 Föremål</h1>
        <p className="text-muted">{items?.length ?? 0} föremål tillgängliga</p>
      </div>

      <div className="item-grid">
        {items?.map((item) => (
          <div key={item.id} className="card">
            <div className="flex-between" style={{ marginBottom: '.5rem' }}>
              <span style={{ fontSize: '1.5rem' }}>{TYPE_EMOJI[item.type] ?? '📦'}</span>
              <span style={{ fontSize: '.75rem', color: 'var(--text-muted)', background: 'var(--surface-2)', padding: '.1rem .5rem', borderRadius: '99px' }}>
                {item.type}
              </span>
            </div>
            <h3 style={{ fontSize: '1rem', marginBottom: '.3rem' }}>{item.name}</h3>
            <p style={{ fontSize: '.8rem', color: 'var(--text-muted)', marginBottom: '.6rem' }}>
              {item.description}
            </p>
            <div className="flex gap-1" style={{ flexWrap: 'wrap' }}>
              {item.strengthBonus     > 0 && <Bonus label="STR" value={item.strengthBonus} />}
              {item.intelligenceBonus > 0 && <Bonus label="INT" value={item.intelligenceBonus} />}
              {item.agilityBonus      > 0 && <Bonus label="AGI" value={item.agilityBonus} />}
              {item.defenseBonus      > 0 && <Bonus label="DEF" value={item.defenseBonus} />}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

function Bonus({ label, value }: { label: string; value: number }) {
  return (
    <span style={{ fontSize: '.75rem', background: 'var(--surface-2)', padding: '.15rem .55rem', borderRadius: '99px', color: 'var(--gold)', border: '1px solid var(--border)' }}>
      {label} +{value}
    </span>
  );
}
