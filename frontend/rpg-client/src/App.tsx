import { NavLink, Route, Routes, useNavigate } from 'react-router-dom';
import { CharactersPage }     from './pages/CharactersPage';
import { CharacterDetailPage } from './pages/CharacterDetailPage';
import { ItemsPage }           from './pages/ItemsPage';
import { LoginPage }           from './pages/LoginPage';
import { tokenStorage }        from './api/apiClient';

export default function App() {
  const navigate   = useNavigate();
  const isLoggedIn = !!tokenStorage.get();

  const handleLogout = () => {
    tokenStorage.clear();
    navigate('/login');
  };

  return (
    <>
      <nav className="navbar">
        <div className="container navbar__inner">
          <NavLink to="/" className="navbar__brand">⚔ RPG World</NavLink>
          {isLoggedIn && (
            <>
              <NavLink to="/"      className="navbar__link" end>Karaktärer</NavLink>
              <NavLink to="/items" className="navbar__link">Föremål</NavLink>
              <button
                onClick={handleLogout}
                style={{
                  marginLeft: 'auto', background: 'transparent', border: '1px solid #c0392b',
                  color: '#c0392b', padding: '0.3rem 0.8rem', borderRadius: 4,
                  cursor: 'pointer', fontSize: '0.85rem',
                }}
              >
                Logga ut
              </button>
            </>
          )}
        </div>
      </nav>

      <main style={{ paddingBottom: '3rem' }}>
        <Routes>
          <Route path="/login"               element={<LoginPage />} />
          <Route path="/"                    element={<CharactersPage />} />
          <Route path="/characters/:id"      element={<CharacterDetailPage />} />
          <Route path="/items"               element={<ItemsPage />} />
        </Routes>
      </main>
    </>
  );
}
