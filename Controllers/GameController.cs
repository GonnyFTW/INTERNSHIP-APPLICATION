using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        
        private static List<Game> games = new List<Game>
        {
            new Game {
                Id = 1,
                Title = "Football",
                Date = DateTime.Now,
                Players = new List<Player>(),
                Rounds = new List<Round>(),  
                TotalRounds = 5,             
                CurrentRound = 1,
                PlayerScore = 0,
                AIScore = 0,
                IsGameOver = false
            }
        };

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

        [HttpPost]
        public IActionResult CreateGame([FromBody] Game newGame)
        {
            newGame.Id = games.Count + 1;
            games.Add(newGame);
            return CreatedAtAction(nameof(GetGame), new { id = newGame.Id }, newGame);
        }

        // Penalty Shooter Game
        [HttpPost("start-penalty-shooter")]
        public IActionResult StartPenaltyShooter([FromBody] int totalRounds)
        {
            var newGame = new Game
            {
                Id = games.Count + 1,
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

            
            for (int i = 0; i < totalRounds; i++)
            {
                newGame.Rounds.Add(new Round
                {
                    RoundNumber = i + 1,     
                    PlayerChoice = string.Empty, 
                    AIChoice = string.Empty,      
                    PlayerScored = false         
                });
            }

            games.Add(newGame);  
            return CreatedAtAction(nameof(GetGame), new { id = newGame.Id }, newGame);
        }

        [HttpPost("{id}/play-round")]
        public IActionResult PlayRound(int id, [FromBody] string playerChoice)
        {
            var game = games.Find(g => g.Id == id);
            if (game == null || game.IsGameOver)
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

            
            if (game.CurrentRound > game.TotalRounds)
            {
                game.IsGameOver = true;
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


        private string GetAIChoice()
        {
            var choices = new List<string> { "left", "center", "right" };
            return choices[new Random().Next(choices.Count)];
        }
    }
}
