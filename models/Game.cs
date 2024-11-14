namespace WebApplication1.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public List<Player> Players { get; set; } 
    }
}
