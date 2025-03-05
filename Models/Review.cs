namespace TheOrchidArchade.Models
{
    public class Review
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? Description { get; set; }

        public int Rating { get; set; }

        public required string GameId { get; set; }

        public Game? Game { get; set; }

        public required string UserId { get; set; }

        public User? User { get; set; }

    }
}
