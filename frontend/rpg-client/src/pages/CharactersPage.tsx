import { useState } from 'react';
import { useCharacters } from '../hooks/useRpgApi';
import { CharacterCard } from '../components/characters/CharacterCard';
import { CreateCharacterModal } from '../components/characters/CreateCharacterModal';
import { ErrorMessage, Spinner } from '../components/ui/Common';

/**
 * Startsida – visar alla karaktärer i ett grid.
 * Håller bara UI-state (modal öppen/stängd).
 * All server-state hanteras av useCharacters() (React Query).
 */
export function CharactersPage() {
  const [showModal, setShowModal] = useState(false);
  const { data: characters, isLoading, error } = useCharacters();

  if (isLoading) return <Spinner />;
  if (error)     return <ErrorMessage message={error.message} />;

  return (
    <div className="container">
      <div className="page-header flex-between">
        <div>
          <h1>⚔ Karaktärer</h1>
          <p className="text-muted">{characters?.length ?? 0} karaktärer i världen</p>
        </div>
        <button className="btn btn-primary" onClick={() => setShowModal(true)}>
          + Ny karaktär
        </button>
      </div>

      {characters?.length === 0 ? (
        <div className="card" style={{ textAlign: 'center', padding: '3rem' }}>
          <p style={{ fontSize: '2rem', marginBottom: '1rem' }}>🏰</p>
          <p className="text-muted">Inga karaktärer än. Skapa din första!</p>
        </div>
      ) : (
        <div className="character-grid">
          {characters?.map((c) => (
            <CharacterCard key={c.id} character={c} />
          ))}
        </div>
      )}

      {showModal && <CreateCharacterModal onClose={() => setShowModal(false)} />}
    </div>
  );
}
