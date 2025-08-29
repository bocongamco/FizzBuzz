import { useState } from 'react';
import { createGame, startSession } from '../api/fizzbuzz';
import type { CreateGameRequest, StartSessionResponse } from '../api/types';

const defaultReq: CreateGameRequest = {
  name: 'FooBooLoo', author: 'Player', min: 1, max: 100,
  rules: [
    { divisor: 7, word: 'Foo', order: 1 },
    { divisor: 11, word: 'Boo', order: 2 },
    { divisor: 103, word: 'Loo', order: 3 },
  ]
};

export default function Home() {
  const [req, setReq] = useState<CreateGameRequest>(defaultReq);
  const [duration, setDuration] = useState(60);
  const [busy, setBusy] = useState(false);

  async function handleStart() {
    try {
      setBusy(true);
      const game = await createGame(req);
      const s: StartSessionResponse = await startSession({ gameId: game.id, durationSeconds: duration });
      sessionStorage.setItem('sessionId', s.sessionId);
      sessionStorage.setItem('firstNumber', String(s.firstNumber));
      sessionStorage.setItem('endsAt', s.endsAt);
      window.location.href = '/play';
    } catch (e: any) {
      alert(e.message ?? 'Failed to start');
    } finally { setBusy(false); }
  }

  return (
    <div className="card p-3">
      <h5 className="mb-3">Create Game</h5>
      <div className="row g-2">
        <div className="col-md-6">
          <label className="form-label">Name</label>
          <input className="form-control" value={req.name}
                 onChange={e=>setReq({...req, name:e.target.value})}/>
        </div>
        <div className="col-md-6">
          <label className="form-label">Author</label>
          <input className="form-control" value={req.author}
                 onChange={e=>setReq({...req, author:e.target.value})}/>
        </div>
        <div className="col-3">
          <label className="form-label">Min</label>
          <input type="number" className="form-control" value={req.min}
                 onChange={e=>setReq({...req, min:+e.target.value})}/>
        </div>
        <div className="col-3">
          <label className="form-label">Max</label>
          <input type="number" className="form-control" value={req.max}
                 onChange={e=>setReq({...req, max:+e.target.value})}/>
        </div>
        <div className="col-3">
          <label className="form-label">Duration (s)</label>
          <input type="number" className="form-control" value={duration}
                 onChange={e=>setDuration(+e.target.value)}/>
        </div>
      </div>

      <hr/>
      <h6>Rules</h6>
      {req.rules.map((r, i) => (
        <div className="row g-2 mb-2" key={i}>
          <div className="col-3">
            <label className="form-label">Divisor</label>
            <input type="number" className="form-control" value={r.divisor}
              onChange={e=>{ const rules=[...req.rules]; rules[i]={...r, divisor:+e.target.value}; setReq({...req, rules}); }}/>
          </div>
          <div className="col-5">
            <label className="form-label">Word</label>
            <input className="form-control" value={r.word}
              onChange={e=>{ const rules=[...req.rules]; rules[i]={...r, word:e.target.value}; setReq({...req, rules}); }}/>
          </div>
          <div className="col-3">
            <label className="form-label">Order</label>
            <input type="number" className="form-control" value={r.order ?? (i+1)}
              onChange={e=>{ const rules=[...req.rules]; rules[i]={...r, order:+e.target.value}; setReq({...req, rules}); }}/>
          </div>
        </div>
      ))}

      <div className="mt-3">
        <button className="btn btn-primary" onClick={handleStart} disabled={busy}>
          {busy ? 'Starting…' : 'Create & Start'}
        </button>
      </div>
    </div>
  );
}
