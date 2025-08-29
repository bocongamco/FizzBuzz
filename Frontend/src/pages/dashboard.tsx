import { useEffect, useMemo, useState } from 'react';
import { listGames, deleteGame, startSession } from '../api/fizzbuzz';
import type { GameListItem } from '../api/types';

export default function Dashboard() {
  const [games, setGames] = useState<GameListItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [duration, setDuration] = useState(60);
  const [busyId, setBusyId] = useState<string | null>(null);

  async function refresh() {
    setLoading(true);
    try {
      const data = await listGames();
      setGames(data);
    } catch (e: any) {
      alert(e.message ?? 'Failed to load games');
    } finally { setLoading(false); }
  }

  useEffect(() => { refresh(); }, []);

  async function onDelete(id: string) {
    if (!confirm('Delete this game (and all its sessions)?')) return;
    try {
      await deleteGame(id);
      setGames(games => games.filter(g => g.id !== id));
    } catch (e: any) {
      alert(e.message ?? 'Delete failed');
    }
  }

  async function onReplay(g: GameListItem) {
    try {
      setBusyId(g.id);
      const res = await startSession({ gameId: g.id, durationSeconds: duration });
      sessionStorage.setItem('sessionId', res.sessionId);
      sessionStorage.setItem('firstNumber', String(res.firstNumber));
      sessionStorage.setItem('endsAt', res.endsAt);
      window.location.href = '/play';
    } catch (e: any) {
      alert(e.message ?? 'Could not start a session');
    } finally { setBusyId(null); }
  }

  const rows = useMemo(() => games.map(g => (
    <tr key={g.id}>
      <td className="text-break">
        <div className="fw-semibold">{g.name}</div>
        <small className="text-muted">by {g.author}</small>
      </td>
      <td className="text-center">{g.rules}</td>
      <td className="text-center">{g.sessions}</td>
      <td className="text-center">{g.bestScore}</td>
      <td className="text-nowrap">{new Date(g.createdAt).toLocaleString()}</td>
      <td className="text-end">
        <div className="input-group input-group-sm" style={{maxWidth: 180}}>
          <span className="input-group-text">Duration</span>
          <input type="number" min={10} className="form-control" value={duration}
                 onChange={e=>setDuration(+e.target.value)} />
          <button className="btn btn-success"
                  onClick={() => onReplay(g)}
                  disabled={busyId === g.id}>
            {busyId === g.id ? 'Starting…' : 'Replay'}
          </button>
          <button className="btn btn-outline-danger" onClick={() => onDelete(g.id)}>
            Delete
          </button>
        </div>
      </td>
    </tr>
  )), [games, duration, busyId]);

  return (
    <div className="card p-3">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h5 className="m-0">Games</h5>
        <a className="btn btn-primary btn-sm" href="/">+ New Game</a>
      </div>

      {loading ? <div>Loading…</div> : (
        games.length === 0 ? <div className="text-muted">No games yet. Create one!</div> : (
          <div className="table-responsive">
            <table className="table align-middle">
              <thead>
                <tr>
                  <th>Game</th>
                  <th className="text-center">Rules</th>
                  <th className="text-center">Sessions</th>
                  <th className="text-center">Best Score</th>
                  <th>Created</th>
                  <th className="text-end">Actions</th>
                </tr>
              </thead>
              <tbody>{rows}</tbody>
            </table>
          </div>
        )
      )}
    </div>
  );
}
