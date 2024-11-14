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
            new Game { Id = 1, Title = "Football", Date = DateTime.Now, Players = new List<Player>() }
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
                return NotFound();
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
    }
}
