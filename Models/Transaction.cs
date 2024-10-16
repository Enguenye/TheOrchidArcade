namespace TheOrchidArchade.Models
{
    public class Transaction
    {

        public int Id { get; set; }

        public required int UserId { get; set; }

        public User? User { get; set; }

        public required int GameId { get; set; }

        public Game? Game { get; set; }

        public required DateTime Date { get; set; }
    }
}
