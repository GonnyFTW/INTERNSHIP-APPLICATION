import './App.css';
import React, { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Home from './components/Home';
import GameScreen from './components/GameScreen';
import Scoreboard from './components/Scoreboard';

function App() {
    const [message, setMessage] = useState('');

    useEffect(() => {
        fetch('https://localhost:7004/api/HelloWorld')
            .then(response => response.text())
            .then(data => setMessage(data))
            .catch(error => console.error('Error fetching data:', error));
    }, []);

    return (
        <Router>
            <nav>
                <ul>
                    <li>
                        <Link to="/">Home</Link>
                    </li>
                    <li>
                        <Link to="/game">Game Screen</Link>
                    </li>
                    <li>
                        <Link to="/scoreboard">Scoreboard</Link>
                    </li>
                </ul>
            </nav>

            <Routes>
                
                <Route
                    path="/"
                    element={
                        <div>
                            <h1>{message}</h1>
                            <Home />
                        </div>
                    }
                />

                
                <Route path="/game" element={<GameScreen />} />

                
                <Route path="/scoreboard" element={<Scoreboard />} />
            </Routes>

        </Router>
    );
}

export default App;
