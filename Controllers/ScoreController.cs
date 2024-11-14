using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        private static List<Score> scores = new List<Score>
        {
            new Score { Id = 1, PlayerId = 1, GameId = 1, Points = 500 }
        };

        [HttpGet]
        public IActionResult GetScores()
        {
            return Ok(scores);
        }

        [HttpGet("{id}")]
        public IActionResult GetScore(int id)
        {
            var score = scores.Find(s => s.Id == id);
            if (score == null)
            {
                return NotFound();
            }
            return Ok(score);
        }

        [HttpPost]
        public IActionResult CreateScore([FromBody] Score newScore)
        {
            newScore.Id = scores.Count + 1;
            scores.Add(newScore);
            return CreatedAtAction(nameof(GetScore), new { id = newScore.Id }, newScore);
        }
    }
}
