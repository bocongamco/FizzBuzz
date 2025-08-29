
import { useEffect, useState } from 'react';
import { submitAnswer, nextNumber, summary } from '../api/fizzbuzz';
import type { SummaryResponse } from '../api/types';

export default function Play() {
  const [sessionId] = useState(() => sessionStorage.getItem('sessionId')!);
  const [current, setCurrent] = useState<number>(() => Number(sessionStorage.getItem('firstNumber')));
  const [endsAt] = useState<number>(() => new Date(sessionStorage.getItem('endsAt')!).getTime());
  const [remaining, setRemaining] = useState<number>(Math.max(0, Math.floor((endsAt - Date.now())/1000)));
  const [answer, setAnswer] = useState('');
  const [msg, setMsg] = useState('');
  const [correct, setCorrect] = useState(0);
  const [incorrect, setIncorrect] = useState(0);
  const [done, setDone] = useState<SummaryResponse | null>(null);

  useEffect(() => {
    const t = setInterval(() => {
      const sec = Math.max(0, Math.floor((endsAt - Date.now())/1000));
      setRemaining(sec);
      if (sec <= 0) { clearInterval(t); finish(); }
    }, 500);
    return () => clearInterval(t);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  async function finish() {
    try { setDone(await summary(sessionId)); } catch {}
  }

  async function onSubmit() {
    if (!answer.trim()) return;
    try {
      const res = await submitAnswer(sessionId, { number: current, answer });
      setCorrect(res.scoreCorrect); setIncorrect(res.scoreIncorrect);
      setMsg(res.isCorrect ? '✅ Correct!' : `❌ Expected: ${res.expected}`);
      setAnswer('');
      const nxt = await nextNumber(sessionId);
      if (nxt === null) await finish(); else setCurrent(nxt.number);
    } catch (e: any) {
      if (e.message === 'expired') await finish(); else setMsg('Error.');
    }
  }

  if (!sessionId) return <div className="alert alert-danger">No session.</div>;
  if (done) return (
    <div className="text-center">
      <h3>Time’s up!</h3>
      <p className="lead">Score: {done.scoreCorrect} / {done.scoreIncorrect} (Total {done.total})</p>
      <a className="btn btn-secondary" href="/">Home</a>
    </div>
  );

  return (
    <div className="mx-auto" style={{maxWidth:520}}>
      <div className="d-flex justify-content-between mb-3">
        <span className="badge bg-primary">Time: {remaining}s</span>
        <span>✅ {correct} &nbsp; ❌ {incorrect}</span>
      </div>
      <div className="card p-4 text-center">
        <div className="display-4 mb-3">{current}</div>
        <input className="form-control form-control-lg mb-3"
               placeholder='Type "FooBoo" or the number'
               value={answer}
               onChange={e=>setAnswer(e.target.value)}
               onKeyDown={e=>e.key==='Enter' && onSubmit()}
               autoFocus />
        <button className="btn btn-success btn-lg" onClick={onSubmit}>Submit</button>
        {msg && <div className="mt-3">{msg}</div>}
      </div>
    </div>
  );
}
