/** Spinner visas medan data laddas. */
export function Spinner() {
  return (
    <div style={{ textAlign: 'center', padding: '3rem', color: 'var(--text-muted)' }}>
      ⏳ Laddar...
    </div>
  );
}

/** Felmeddelande från API eller nätverksfel. */
export function ErrorMessage({ message }: { message: string }) {
  return (
    <div className="card" style={{ borderColor: 'var(--red)', marginTop: '1rem' }}>
      <p className="error-msg">⚠ {message}</p>
    </div>
  );
}

/** Hälsobar – färg beror på procentandel kvar. */
export function HealthBar({ current, max }: { current: number; max: number }) {
  const pct = max > 0 ? Math.round((current / max) * 100) : 0;
  const color = pct > 60 ? 'var(--green)' : pct > 30 ? 'var(--gold)' : 'var(--red)';
  return (
    <div>
      <div className="flex-between" style={{ marginBottom: '4px', fontSize: '.8rem' }}>
        <span style={{ color: 'var(--text-muted)' }}>HP</span>
        <span>{current} / {max}</span>
      </div>
      <div className="health-bar">
        <div className="health-bar__fill" style={{ width: `${pct}%`, background: color }} />
      </div>
    </div>
  );
}

/** Klassemärke med färg per klass. */
export function ClassBadge({ className }: { className: string }) {
  const cls = className.toLowerCase();
  return <span className={`badge badge-${cls}`}>{className}</span>;
}

/** Enkel stat-ruta. */
export function StatBox({ label, value }: { label: string; value: number }) {
  return (
    <div className="stat-box">
      <div className="stat-box__value">{value}</div>
      <div className="stat-box__label">{label}</div>
    </div>
  );
}
