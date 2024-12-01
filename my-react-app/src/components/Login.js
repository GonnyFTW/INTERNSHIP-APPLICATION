
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../authStyles.css';

function Login({ setIsLoggedIn }) {  
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('https://localhost:7004/api/Auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    username: username,
                    passwordHash: password,
                }),
            });

            if (!response.ok) {
                const errorData = await response.text();
                setError(errorData);
                return;
            }

            const data = await response.json();
            alert('Login successful!');
            localStorage.setItem('authToken', data.token); 
            localStorage.setItem('isLoggedIn', 'true'); 

            
            setIsLoggedIn(true); 

            console.log('Token:', data.token);
            navigate('/'); 
        } catch (error) {
            console.error('Error during login:', error);
            setError('An error occurred while logging in. Please try again.');
        }
    };

    return (
        <div>
            <h1>Login</h1>
            <form onSubmit={handleLogin}>
                <div>
                    <label>Username:</label>
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>Password:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit">Login</button>
            </form>

            {error && <div style={{ color: 'red' }}>{error}</div>}
        </div>
    );
}

export default Login;
