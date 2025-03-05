namespace TheOrchidArchade.Models
{
    public class Transaction
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string UserId { get; set; }

        public User? User { get; set; }

        public required string GameId { get; set; }

        public Game? Game { get; set; }

        public required DateTime Date { get; set; }
    }
}
