import React, { useEffect, useState } from 'react';
import '../authStyles.css';
function Scoreboard() {
    const [scores, setScores] = useState([]);
    const [leaderboard, setLeaderboard] = useState([]);
    const [loading, setLoading] = useState(true);

    const fetchScores = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/score');  
            const data = await response.json();
            setScores(data);
        } catch (error) {
            console.error('Error fetching scores:', error);
        }
    };

    
    const fetchLeaderboard = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/score/leaderboard');  
            const data = await response.json();
            setLeaderboard(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching leaderboard:', error);
        }
    };

    useEffect(() => {
        fetchScores();
        fetchLeaderboard();
    }, []);

    return (
        <div>
            <h1>Scoreboard</h1>
            

            {loading ? (
                <p>Loading...</p>
            ) : (
                <div>
                    <h2>Leaderboard</h2>
                    <table>
                        <thead>
                            <tr>
                                <th>Username</th>
                                <th>Email</th>
                                <th>Points</th>
                                <th>Result</th>
                            </tr>
                        </thead>
                        <tbody>
                            {leaderboard.map((score, index) => (
                                <tr key={index}>
                                    <td>{score.Username}</td>
                                    <td>{score.Email}</td>
                                    <td>{score.Points}</td>
                                    <td>{score.ResultMessage}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>

                    <h2>All Scores</h2>
                    <table>
                        <thead>
                            <tr>
                                <th>Username</th>
                                <th>Email</th>
                                <th>Points</th>
                                <th>Result</th>
                            </tr>
                        </thead>
                        <tbody>
                            {scores.map((score, index) => (
                                <tr key={index}>
                                    <td>{score.Username}</td>
                                    <td>{score.Email}</td>
                                    <td>{score.Points}</td>
                                    <td>{score.ResultMessage}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    );
}

export default Scoreboard;
