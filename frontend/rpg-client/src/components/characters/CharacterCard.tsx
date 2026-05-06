import { useNavigate } from 'react-router-dom';
import type { Character } from '../../types/api.types';
import { useDeleteCharacter, useLevelUp } from '../../hooks/useRpgApi';
import { ClassBadge, HealthBar } from '../ui/Common';

interface Props {
  character: Character;
}

/**
 * Karaktärskort i listvyn.
 * Visar nyckelinfo och ger snabbåtgärder (level up, ta bort).
 * Klicka på kortet → navigera till detaljsidan.
 */
export function CharacterCard({ character: c }: Props) {
  const navigate      = useNavigate();
  const levelUp       = useLevelUp();
  const deleteChar    = useDeleteCharacter();

  function handleLevelUp(e: React.MouseEvent) {
    e.stopPropagation(); // Hindra navigering till detaljsidan
    levelUp.mutate(c.id);
  }

  function handleDelete(e: React.MouseEvent) {
    e.stopPropagation();
    if (confirm(`Ta bort ${c.name}?`)) deleteChar.mutate(c.id);
  }

  return (
    <div
      className="card"
      onClick={() => navigate(`/characters/${c.id}`)}
      style={{ cursor: 'pointer', transition: 'border-color .2s' }}
      onMouseEnter={(e) => (e.currentTarget.style.borderColor = 'var(--gold)')}
      onMouseLeave={(e) => (e.currentTarget.style.borderColor = 'var(--border)')}
    >
      {/* Header */}
      <div className="flex-between" style={{ marginBottom: '.8rem' }}>
        <div>
          <h3 style={{ fontWeight: 700 }}>{c.name}</h3>
          <div className="flex gap-1 mt-1">
            <ClassBadge className={c.class} />
            <span className="badge" style={{ background: 'var(--surface-2)', border: '1px solid var(--border)' }}>
              Niv. {c.level}
            </span>
          </div>
        </div>
        <span style={{ fontSize: '2rem' }}>{classEmoji(c.class)}</span>
      </div>

      {/* Hälsobar */}
      <HealthBar current={c.currentHealth} max={c.maxHealth} />

      {/* Stats (kompakt) */}
      <div className="flex gap-2 mt-2" style={{ fontSize: '.8rem', color: 'var(--text-muted)' }}>
        <span>⚔ {c.strength}</span>
        <span>🔮 {c.intelligence}</span>
        <span>💨 {c.agility}</span>
        <span>🛡 {c.defense}</span>
        <span>🎒 {c.inventory.length}</span>
      </div>

      <hr className="divider" />

      {/* Actions */}
      <div className="flex gap-1">
        <button
          className="btn btn-success btn-sm"
          onClick={handleLevelUp}
          disabled={levelUp.isPending}
        >
          ⬆ Level up
        </button>
        <button
          className="btn btn-danger btn-sm"
          onClick={handleDelete}
          disabled={deleteChar.isPending}
          style={{ marginLeft: 'auto' }}
        >
          🗑 Ta bort
        </button>
      </div>
    </div>
  );
}

function classEmoji(cls: string) {
  const map: Record<string, string> = {
    Warrior: '🗡️', Mage: '🔮', Rogue: '🗡', Paladin: '⚔️', Ranger: '🏹',
  };
  return map[cls] ?? '🧙';
}
