import { useState } from 'react';
import { CHARACTER_CLASSES } from '../../types/api.types';
import { useCreateCharacter } from '../../hooks/useRpgApi';

interface Props {
  onClose: () => void;
}

/**
 * Modal för att skapa en ny karaktär.
 *
 * useState för lokalt formulärstate – React Query sköter
 * serverstate (cache-invalidering) via useCreateCharacter.
 */
export function CreateCharacterModal({ onClose }: Props) {
  const [name, setName] = useState('');
  const [cls, setCls]   = useState(1);
  const { mutate, isPending, error } = useCreateCharacter();

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    mutate(
      { name: name.trim(), characterClass: cls },
      { onSuccess: onClose }  // Stäng modalen när det lyckas
    );
  }

  return (
    // Backdrop – klicka utanför för att stänga
    <div
      onClick={onClose}
      style={{
        position: 'fixed', inset: 0, background: 'rgba(0,0,0,.7)',
        display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 200,
      }}
    >
      <div
        className="card"
        style={{ width: '100%', maxWidth: '420px' }}
        onClick={(e) => e.stopPropagation()} // Hindra stängning vid klick inuti
      >
        <h2 style={{ marginBottom: '1.2rem', color: 'var(--gold)' }}>⚔ Ny karaktär</h2>

        <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          <div className="form-group">
            <label className="form-label">Namn</label>
            <input
              className="input"
              value={name}
              onChange={(e) => setName(e.target.value)}
              placeholder="Skriv ett namn..."
              autoFocus
              required
              maxLength={100}
            />
          </div>

          <div className="form-group">
            <label className="form-label">Klass</label>
            <select
              className="select"
              value={cls}
              onChange={(e) => setCls(Number(e.target.value))}
            >
              {CHARACTER_CLASSES.map((c) => (
                <option key={c.value} value={c.value}>{c.label}</option>
              ))}
            </select>
          </div>

          {error && <p className="error-msg">⚠ {error.message}</p>}

          <div className="flex gap-1" style={{ justifyContent: 'flex-end', marginTop: '.5rem' }}>
            <button type="button" className="btn btn-ghost" onClick={onClose}>Avbryt</button>
            <button type="submit" className="btn btn-primary" disabled={isPending || !name.trim()}>
              {isPending ? '...' : 'Skapa'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
