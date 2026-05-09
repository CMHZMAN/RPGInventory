import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authApi } from '../api/rpgApi';
import { tokenStorage } from '../api/apiClient';

export function LoginPage() {
  const navigate = useNavigate();
  const [tab, setTab]           = useState<'login' | 'register'>('login');
  const [username, setUsername] = useState('');
  const [email, setEmail]       = useState('');
  const [password, setPassword] = useState('');
  const [error, setError]       = useState('');
  const [loading, setLoading]   = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      const result = tab === 'login'
        ? await authApi.login(username, password)
        : await authApi.register(username, email, password);

      tokenStorage.set(result.token);
      navigate('/');
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Något gick fel.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ maxWidth: 380, margin: '6rem auto', padding: '0 1rem' }}>
      <h1 style={{ textAlign: 'center', marginBottom: '1.5rem' }}>⚔ RPG World</h1>

      {/* Tab-växlare */}
      <div style={{ display: 'flex', marginBottom: '1.5rem', borderBottom: '2px solid #333' }}>
        {(['login', 'register'] as const).map((t) => (
          <button
            key={t}
            onClick={() => { setTab(t); setError(''); }}
            style={{
              flex: 1, padding: '0.6rem', border: 'none', cursor: 'pointer',
              background: tab === t ? '#c0392b' : 'transparent',
              color: tab === t ? '#fff' : '#aaa',
              fontWeight: tab === t ? 700 : 400,
            }}
          >
            {t === 'login' ? 'Logga in' : 'Registrera'}
          </button>
        ))}
      </div>

      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '0.8rem' }}>
        <input
          placeholder="Användarnamn"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          required
          style={inputStyle}
        />
        {tab === 'register' && (
          <input
            placeholder="E-post"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            style={inputStyle}
          />
        )}
        <input
          placeholder="Lösenord (minst 6 tecken)"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          style={inputStyle}
        />

        {error && (
          <p style={{ color: '#e74c3c', margin: 0, fontSize: '0.9rem' }}>⚠ {error}</p>
        )}

        <button
          type="submit"
          disabled={loading}
          style={{
            padding: '0.75rem', background: '#c0392b', color: '#fff',
            border: 'none', borderRadius: 4, cursor: loading ? 'not-allowed' : 'pointer',
            fontWeight: 700, opacity: loading ? 0.7 : 1,
          }}
        >
          {loading ? 'Laddar...' : tab === 'login' ? 'Logga in' : 'Skapa konto'}
        </button>
      </form>
    </div>
  );
}

const inputStyle: React.CSSProperties = {
  padding: '0.65rem 0.75rem',
  background: '#1a1a1a',
  border: '1px solid #444',
  borderRadius: 4,
  color: '#fff',
  fontSize: '1rem',
};
