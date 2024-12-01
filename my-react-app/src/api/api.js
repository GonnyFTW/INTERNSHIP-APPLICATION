const BASE_URL = 'https://localhost:7004/api';

export const fetchGame = async (gameId) => {
    const response = await fetch(`${BASE_URL}/Game/${gameId}`);
    if (!response.ok) {
        throw new Error('Failed to fetch game');
    }
    return response.json();
};


export const createGame = async (totalRounds = 5) => {
    const response = await fetch(`${BASE_URL}/Game/create-game`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(totalRounds),
    });
    if (!response.ok) {
        throw new Error('Failed to start new game');
    }
    return response.json(); 
};


export const setRounds = async (gameId, totalRounds) => {
    const response = await fetch(`${BASE_URL}/Game/set-rounds/${gameId}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(totalRounds),
    });
    if (!response.ok) {
        throw new Error('Failed to set rounds');
    }
    return response.json(); 
};


export const playRound = async (gameId, playerChoice) => {
    try {
        const response = await fetch(`${BASE_URL}/Game/${gameId}/play-round`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(playerChoice),
        });

        if (!response.ok) {
            const errorText = await response.text(); 
            console.error('Error Response:', errorText); 
            throw new Error(`Failed to play round: ${errorText}`);
        }

        return response.json();
    } catch (err) {
        console.error('Error during playRound:', err); 
        throw err; 
    }
};

export const getGameState = async (gameId) => {
    const response = await fetch(`${BASE_URL}/Game/${gameId}/state`);
    if (!response.ok) {
        throw new Error('Failed to fetch game state');
    }
    return response.json();
};


export const restartGame = async (rounds) => {
    const response = await fetch(`${BASE_URL}/Game/create-game`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(rounds),
    });
    if (!response.ok) {
        throw new Error('Failed to restart game');
    }
    return response.json();
};
