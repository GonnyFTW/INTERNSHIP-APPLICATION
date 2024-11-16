using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private static List<Player> players = new List<Player>
        {
            new Player { Id = 1, Username = "Agon Jakupi", Age = 21, Email = "jakupigoni123@gmail.com" },
            new Player { Id = 2, Username = "Tose Proeski", Age = 26, Email = "tose@yahoo.com" }
        };

        [HttpGet]
        public IActionResult GetPlayers()
        {
            return Ok(players);
        }

        [HttpGet("{id}")]
        public IActionResult GetPlayer(int id)
        {
            var player = players.Find(p => p.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            return Ok(player);
        }

        [HttpPost]
        public IActionResult CreatePlayer([FromBody] Player newPlayer)
        {
            newPlayer.Id = players.Count + 1;
            players.Add(newPlayer);
            return CreatedAtAction(nameof(GetPlayer), new { id = newPlayer.Id }, newPlayer);
        }
    }
}
