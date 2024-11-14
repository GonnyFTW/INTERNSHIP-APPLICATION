import React, { useEffect, useState } from 'react';

function App() {
    const [message, setMessage] = useState('');

    useEffect(() => {
        fetch('https://localhost:7004/api/HelloWorld')
            .then(response => response.text())
            .then(data => setMessage(data))
            .catch(error => console.error('Error fetching data:', error));
            
    }, []);

    return (
        <div>
            <h1>{message}</h1>
        </div>
    );
}

export default App;
