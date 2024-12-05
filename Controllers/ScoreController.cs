using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        public static List<Score> scores = new List<Score>();

        [HttpGet]
        public IActionResult GetScores()
        {
            if (scores.Count == 0)
            {
                return NotFound("No scores available.");
            }

            var result = scores.Select(s => new
            {
                s.Username,
                s.Email,
                s.Points,
                s.ResultMessage
            }).ToList();

            return Ok(result);
        }

        [HttpGet("leaderboard")]
        public IActionResult GetLeaderboard()
        {
            var leaderboard = scores
                .OrderByDescending(s => s.Points)
                .Select(s => new
                {
                    s.Username,
                    s.Email,
                    s.Points,
                    s.ResultMessage
                })
                .ToList();

            return Ok(leaderboard);
        }



        [HttpGet("{gameId}/{playerId}")]
        public IActionResult GetScore(int gameId, int playerId)
        {
            var score = scores.FirstOrDefault(s => s.GameId == gameId && s.PlayerId == playerId);
            if (score == null)
            {
                return NotFound("Score not found.");
            }
            return Ok(new
            {
                score.PlayerId,
                score.GameId,
                score.Points 
            });
        }

        [HttpPost]
        public IActionResult CreateScore([FromBody] Score score)
        {
            scores.Add(score);
            return CreatedAtAction(nameof(GetScore), new { gameId = score.GameId, playerId = score.PlayerId }, score);
        }
    }
}