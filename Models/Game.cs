namespace TheOrchidArchade.Models
{
    public class Game
    {
        public required int Id { get; set; }

        public required string Title { get; set; }

        public required string CoverImage { get; set; }

        public required string Description { get; set; }

        public required string Genre { get; set; }

        public required double Price { get; set; }

        public required double Revenue { get; set; }

        public required int DeveloperId { get; set; }

        public required User Developer { get; set; }
    }
}
