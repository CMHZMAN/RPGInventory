import { NavLink, Route, Routes } from 'react-router-dom';
import { CharactersPage }     from './pages/CharactersPage';
import { CharacterDetailPage } from './pages/CharacterDetailPage';
import { ItemsPage }           from './pages/ItemsPage';

/**
 * App = applikationens rotnod.
 * Definierar navigationsstruktur och vilken komponent
 * som renderas för varje URL-path.
 *
 * React Router v6: <Routes> + <Route element={...}> ersätter
 * den gamla <Switch>-syntaxen.
 */
export default function App() {
  return (
    <>
      {/* ── Navigation ─────────────────────────── */}
      <nav className="navbar">
        <div className="container navbar__inner">
          <NavLink to="/" className="navbar__brand">⚔ RPG World</NavLink>
          <NavLink to="/"      className="navbar__link" end>Karaktärer</NavLink>
          <NavLink to="/items" className="navbar__link">Föremål</NavLink>
        </div>
      </nav>

      {/* ── Sidor ──────────────────────────────── */}
      <main style={{ paddingBottom: '3rem' }}>
        <Routes>
          <Route path="/"                   element={<CharactersPage />} />
          <Route path="/characters/:id"     element={<CharacterDetailPage />} />
          <Route path="/items"              element={<ItemsPage />} />
        </Routes>
      </main>
    </>
  );
}
