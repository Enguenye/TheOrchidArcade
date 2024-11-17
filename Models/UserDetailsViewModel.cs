namespace TheOrchidArchade.Models
{
    public class UserDetailsViewModel
    {
        public User? User { get; set; }
        public List<Game>? Games { get; set; }

        public List<Game>? CreatedGames { get; set; }

        public List<Review>? Reviews { get; set; }
    }
}
