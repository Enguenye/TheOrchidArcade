namespace TheOrchidArchade.Models
{
    public class Transaction
    {
        public required int Id { get; set; }

        public required int UserId { get; set; }

        public required User User { get; set; }

        public required int GameId { get; set; }

        public required Game Game { get; set; }

        public required DateTime Date { get; set; }
    }
}
