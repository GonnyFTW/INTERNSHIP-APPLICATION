import './App.css';
import React, { useEffect, useState } from 'react';
import { Routes, Route, Link, Navigate, useLocation } from 'react-router-dom'; 
import Home from './components/Home';
import GameScreen from './components/GameScreen';
import Scoreboard from './components/Scoreboard';
import Register from './components/Register';
import Login from './components/Login';

function App() {
    const [message, setMessage] = useState('');
    const [isRegistered, setIsRegistered] = useState(false);
    const [isLoggedIn, setIsLoggedIn] = useState(localStorage.getItem('isLoggedIn') === 'true');
    const location = useLocation();

    useEffect(() => {
        fetch('https://localhost:7004/api/HelloWorld')
            .then(response => response.text())
            .then(data => setMessage(data))
            .catch(error => console.error('Error fetching data:', error));

        const userRegistered = localStorage.getItem("userRegistered") === "true";
        const userLoggedIn = localStorage.getItem("isLoggedIn") === "true";
        console.log('Is Logged In:', userLoggedIn);
        setIsLoggedIn(userLoggedIn);

        setIsRegistered(userRegistered);
        setIsLoggedIn(userLoggedIn);
    }, []);

    const handleLogout = () => {
        localStorage.removeItem("isLoggedIn");
        localStorage.removeItem("userRegistered");
        setIsLoggedIn(false);
        setIsRegistered(false);
    };

    const isAuthPage = location.pathname === '/register' || location.pathname === '/login';

    return (
        
        <>
            
            {!isAuthPage && (
                <nav>
                    <ul>
                        {isLoggedIn && (
                            <>
                                <li>
                                    <Link to="/">Home</Link>
                                </li>
                                <li>
                                    <Link to="/game">Game Screen</Link>
                                </li>
                                <li>
                                    <Link to="/scoreboard">Scoreboard</Link>
                                </li>
                            </>
                        )}

                        {isLoggedIn ? (
                            <li>
                                <button className="logout-button" onClick={handleLogout}>Log Out</button>
                            </li>
                        ) : (
                            <>
                                <li></li>
                                <li></li>
                            </>
                        )}
                    </ul>
                </nav>
            )}

            <Routes>
                <Route
                    path="/"
                    element={
                        isLoggedIn ? (
                            <div>
                                <h1>{message}</h1>
                                <Home />
                            </div>
                        ) : (
                            <Navigate to="/register" />
                        )
                    }
                />

                <Route
                    path="/game"
                    element={isLoggedIn ? <GameScreen /> : <Navigate to="/register" />}
                />

                <Route
                    path="/scoreboard"
                    element={isLoggedIn ? <Scoreboard /> : <Navigate to="/register" />}
                />

                <Route
                    path="/register"
                    element={isRegistered ? <Navigate to="/login" /> : <Register />}
                />

                <Route
                    path="/login"
                    element={isLoggedIn ? <Navigate to="/" /> : <Login setIsLoggedIn={setIsLoggedIn} />}
                />
            </Routes>
        </>
    );
}

export default App;
