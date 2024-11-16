import React from 'react';
import { useNavigate } from 'react-router-dom';

function Home() {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('authToken'); 
        navigate('/register'); 
    };

    return (
        <div>
            <h1>Welcome to the Home Page!</h1>
            <p>Welcome to the main page!</p>
            
        </div>
    );
}

export default Home;
