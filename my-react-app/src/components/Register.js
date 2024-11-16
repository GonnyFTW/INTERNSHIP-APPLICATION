import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../authStyles.css';

function Register() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [age, setAge] = useState('');
    const [email, setEmail] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('https://localhost:7004/api/Auth/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    username: username,
                    passwordHash: password,
                    age: age,
                    email: email,
                    id: 0,
                }),
            });

            if (!response.ok) {
                const contentType = response.headers.get('Content-Type');
                if (contentType && contentType.includes('application/json')) {
                    const errorData = await response.json();
                    setError(errorData.message);
                } else {
                    const errorData = await response.text();
                    setError(errorData);
                }
                return;
            }

            const data = await response.json();
            alert(data.message);
            localStorage.setItem("userRegistered", "true");
            console.log(data);
            navigate('/login');
        } catch (error) {
            console.error('Error during registration:', error);
            setError('An error occurred while registering. Please try again.');
        }
    };

    return (
        <div>
            <h1>Register</h1>
            <form onSubmit={handleRegister}>
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
                <div>
                    <label>Age:</label>
                    <input
                        type="number"
                        value={age}
                        onChange={(e) => setAge(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>Email:</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <button type="submit">Register</button>
            </form>

            {error && <div style={{ color: 'red' }}>{error}</div>}

            <p>
                Already have an account?
                <a href="/login">Login here</a>
            </p>
        </div>
    );
}

export default Register;
