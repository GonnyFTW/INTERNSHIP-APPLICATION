using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private static List<Game> games = new List<Game>();
        private static int nextId = 1;  

        [HttpGet]
        public IActionResult GetGames()
        {
            return Ok(games);
        }

        [HttpGet("{id}")]
        public IActionResult GetGame(int id)
        {
            var game = games.Find(g => g.Id == id);
            if (game == null)
            {
                return NotFound("Game not found.");
            }
            return Ok(game);
        }

        [HttpPost("create-game")]
        public IActionResult CreateGame([FromBody] int totalRounds)
        {
            var newGame = new Game
            {
                Id = nextId++,  
                Title = "Penalty Shooter",
                Date = DateTime.Now,
                Players = new List<Player>(),
                TotalRounds = totalRounds,
                CurrentRound = 1,
                PlayerScore = 0,
                AIScore = 0,
                IsGameOver = false,
                Rounds = new List<Round>()
            };

            games.Add(newGame);  
            return CreatedAtAction(nameof(GetGame), new { id = newGame.Id }, newGame);  
        }

        [HttpPost("set-rounds/{gameId}")]
        public IActionResult SetRounds(int gameId, [FromBody] int totalRounds)
        {
            var game = games.FirstOrDefault(g => g.Id == gameId);
            if (game == null)
            {
                return NotFound($"Game with ID {gameId} not found.");
            }

            game.Rounds.Clear();  
            for (int i = 0; i < totalRounds; i++)
            {
                game.Rounds.Add(new Round
                {
                    RoundNumber = i + 1,
                    PlayerChoice = string.Empty,
                    AIChoice = string.Empty,
                    PlayerScored = false
                });
            }

            game.TotalRounds = totalRounds;  
            return Ok(game);  
        }

        [HttpPost("{id}/play-round")]
        public IActionResult PlayRound(int id, [FromBody] string playerChoice)
        {
            var game = games.Find(g => g.Id == id);
            if (game == null || game.CurrentRound > game.TotalRounds)
            {
                return BadRequest("Invalid game or the game is already over.");
            }

            var validChoices = new[] { "left", "center", "right" };
            if (Array.IndexOf(validChoices, playerChoice.ToLower()) == -1)
            {
                return BadRequest("Invalid choice! Please choose between 'left', 'center', or 'right'.");
            }

            var aiChoices = new[] { "left", "center", "right" };
            var random = new Random();
            var aiChoice = aiChoices[random.Next(aiChoices.Length)];

            bool playerScored = playerChoice != aiChoice;
            bool aiSavedBall = !playerScored;

            if (playerScored)
                game.PlayerScore++;
            else
                game.AIScore++;

            var round = new Round
            {
                RoundNumber = game.CurrentRound,
                PlayerChoice = playerChoice,
                AIChoice = aiChoice,
                PlayerScored = playerScored
            };

            game.Rounds.Add(round);
            game.CurrentRound++;

            Console.WriteLine($"Round {game.CurrentRound}/{game.TotalRounds}, Player Choice: {playerChoice}, AI Choice: {aiChoice}, Player Scored: {playerScored}");

            if (game.CurrentRound == game.TotalRounds)
            {
                game.IsGameOver = true;
                Console.WriteLine("Game Over: True");
            }

            var resultMessage = playerScored
                ? "Player scored!"
                : "AI saved the ball!";

            return Ok(new
            {
                game.CurrentRound,
                game.TotalRounds,
                game.PlayerScore,
                game.AIScore,
                game.IsGameOver,
                resultMessage
            });
        }

        [HttpGet("{id}/state")]
        public IActionResult GetGameState(int id)
        {
            var game = games.Find(g => g.Id == id);
            if (game == null)
            {
                return NotFound("Game not found.");
            }

            string resultMessage = string.Empty;
            if (game.IsGameOver)
            {
                if (game.PlayerScore > game.AIScore)
                {
                    resultMessage = "You won the game!";
                }
                else if (game.PlayerScore < game.AIScore)
                {
                    resultMessage = "You lost the game.";
                }
                else
                {
                    resultMessage = "The game ended in a tie.";
                }
            }

            return Ok(new
            {
                CurrentRound = game.CurrentRound,
                TotalRounds = game.TotalRounds,
                PlayerScore = game.PlayerScore,
                AIScore = game.AIScore,
                IsGameOver = game.IsGameOver,
                ResultMessage = resultMessage
            });
        }
    }
}
