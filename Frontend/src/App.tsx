// src/App.tsx
import { Routes, Route, Link } from 'react-router-dom';
import Home from './pages/home';
import Play from './pages/play';
import Dashboard from './pages/dashboard'; 
export default function App() {
  return (
    <div className="shell">
      <header className="navbar">
        <div className="container nav-inner">
          <Link to="/" className="brand">FizzBuzz</Link>

          <nav className="nav-links">
            <Link to="/dashboard">Dashboard</Link>
            <a href="/swagger" target="_blank" rel="noreferrer">API Docs</a>
          </nav>
        </div>
      </header>

      <main className="main">
        <div className="container">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/play" element={<Play />} />
            <Route path="/dashboard" element={<Dashboard />} />
          </Routes>
        
        </div>
      </main>
    </div>
  );
}
