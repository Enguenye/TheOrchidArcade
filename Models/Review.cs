namespace TheOrchidArchade.Models
{
    public class Review
    {
        public required int Id { get; set; }

        public required string Description { get; set; }

        public required int Rating { get; set; }

        public required int GameId { get; set; }

        public required Game Game { get; set; }

        public required int UserId { get; set; }

        public required User User { get; set; }
    }
}
