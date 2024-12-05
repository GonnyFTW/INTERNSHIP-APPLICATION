namespace WebApplication1.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public List<Player> Players { get; set; } = new();
        public int TotalRounds { get; set; } = 5;
        public int CurrentRound { get; set; } = 1;
        public int PlayerScore { get; set; } = 0;
        public int AIScore { get; set; } = 0;
        public bool IsGameOver { get; set; } = false;
        public List<Round> Rounds { get; set; } = new();
    }

    public class Round
    {
        public int RoundNumber { get; set; }
        public string PlayerChoice { get; set; }
        public string AIChoice { get; set; }
        public bool PlayerScored { get; set; }
    }
}
