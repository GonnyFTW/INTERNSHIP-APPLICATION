﻿using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Collections.Generic;
using BCrypt.Net;
using System;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public static Dictionary<string, Player> loggedInPlayers = new();

        private static List<Player> players = new();

        public class LoginRequest
        {
            public string Username { get; set; }
            public string PasswordHash { get; set; }
        }

        private string GenerateJwtToken(Player player)
        {
            return Guid.NewGuid().ToString();
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Player player)
        {
            if (players.Exists(p => p.Username == player.Username || p.Email == player.Email))
            {
                return BadRequest(new { message = "Username or Email already exists." });
            }

            player.PasswordHash = BCrypt.Net.BCrypt.HashPassword(player.PasswordHash);
            player.Id = players.Count + 1;
            players.Add(player);

            return Ok(new { message = "Registration successful!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var existingPlayer = players.Find(p => p.Username == loginRequest.Username);

            if (existingPlayer == null)
            {
                return Unauthorized("Username does not exist.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.PasswordHash, existingPlayer.PasswordHash))
            {
                return Unauthorized("Invalid password.");
            }

            HttpContext.Session.SetString("LoggedInPlayer", existingPlayer.Username);

            loggedInPlayers[existingPlayer.Username] = existingPlayer;

            Console.WriteLine("Logged in player: " + HttpContext.Session.GetString("LoggedInPlayer"));

            return Ok(new { message = "Login successful!" });
        }
        [HttpGet("all")]
        public IActionResult GetAllPlayers()
        {
            return Ok(players);
        }
    }
}
