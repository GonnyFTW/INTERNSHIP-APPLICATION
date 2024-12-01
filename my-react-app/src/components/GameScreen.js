import React, { useState, useEffect } from 'react';
import { fetchGame, createGame, playRound, setRounds, restartGame } from '../api/api';

function GameScreen() {
    const [game, setGame] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [rounds, setRoundsInput] = useState(5); 
    const [gameId, setGameId] = useState(null); 

    // Fetch game on component mount
    useEffect(() => {
        const loadGame = async () => {
            try {
                if (!gameId) {
                    const newGame = await createGame(rounds); 
                    setGameId(newGame.id); 
                    setGame(newGame); 
                } else {
                    const data = await fetchGame(gameId); 
                    setGame(data); 
                }
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        loadGame();
    }, [gameId, rounds]);

    const handleCreateGame = async () => {
        try {
            const newGame = await createGame(rounds); 
            setGameId(newGame.id); 
            setGame(newGame); 
        } catch (err) {
            setError(err.message);
        }
    };

    const handleSetRounds = async () => {
        if (!gameId) {
            console.error("Game ID is not defined");
            return;
        }

        try {
            const updatedGame = await setRounds(gameId, rounds); 
            setGame(updatedGame); 
        } catch (err) {
            setError(err.message);
        }
    };

    const handlePlayRound = async (playerChoice) => {
        if (!game || !game.id || game.isGameOver) {
            setError("The game is already over or invalid game ID.");
            return;
        }

        try {
            const updatedGame = await playRound(game.id, playerChoice);
            console.log("Before update:", game.isGameOver); 
            setGame(updatedGame);
            console.log("After update:", game.isGameOver); 
        } catch (err) {
            console.error("Error playing round:", err);
            setError("An error occurred while playing the round. Please try again.");
        }
    };

    const handleRestartGame = async () => {
        try {
            const newGame = await restartGame(rounds); 
            setGameId(newGame.id); 
            setGame(newGame); 
        } catch (err) {
            setError(err.message);
        }
    };

    const handleRoundsChange = (e) => {
        setRoundsInput(e.target.value); 
    };

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error: {error}</p>;

    return (
        <div>
            <h1>Game: {game.title}</h1>
            <p>Date: {new Date(game.date).toLocaleString()}</p>
            <p>Total Rounds: {game.totalRounds}</p>
            <p>Current Round: {game.currentRound}</p>
            <p>Player Score: {game.playerScore}</p>
            <p>AI Score: {game.aiScore}</p>
            <p>Game Over: {game.isGameOver ? 'Yes' : 'No'}</p>

            
            <div>
                <button onClick={handleCreateGame}>Create New Game</button>
            </div>

            
            <div>
                <label>
                    Number of Rounds:
                    <input
                        type="number"
                        value={rounds}
                        onChange={handleRoundsChange}
                        min="1"
                    />
                </label>
                <button onClick={handleSetRounds}>Set Rounds</button>
            </div>

            
            <div>
                <h3>Take Your Shot:</h3>
                <button onClick={() => handlePlayRound('left')} disabled={game.isGameOver}>Shoot Left</button>
                <button onClick={() => handlePlayRound('center')} disabled={game.isGameOver}>Shoot Center</button>
                <button onClick={() => handlePlayRound('right')} disabled={game.isGameOver}>Shoot Right</button>
            </div>

            
            <div>
                <button onClick={handleRestartGame}>Restart Game</button>
            </div>

            
            {game.isGameOver && (
                <div>
                    <h2>Game Over!</h2>
                    <p>{game.playerScore > game.aiScore ? "You won the game!" : game.playerScore < game.aiScore ? "AI won the game!" : "The game ended in a tie."}</p>
                </div>
            )}
        </div>
    );
}

export default GameScreen;
