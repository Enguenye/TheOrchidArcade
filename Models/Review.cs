namespace TheOrchidArchade.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public int Rating { get; set; }

        public required int GameId { get; set; }

        public Game? Game { get; set; }

        public required int UserId { get; set; }

        public User? User { get; set; }

    }
}
