namespace WebApplication1.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public int Points { get; set; }
        public string Username { get; set; } 
        public string Email { get; set; } 
        public string ResultMessage { get; set; } 
    }
}