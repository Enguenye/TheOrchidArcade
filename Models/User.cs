namespace TheOrchidArchade.Models
{
    public class User
    {
        public required int Id { get; set; }

        public required string Email { get; set; }

        public string? Password { get; set; }

        public required string Username { get; set; }

        public double? revenue { get; set; }

        public required bool isDeveloper { get; set; }
    }
}
