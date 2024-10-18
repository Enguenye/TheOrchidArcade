namespace TheOrchidArchade.Models
{
    public class Game
    {
        public required int Id { get; set; }

        public string Title { get; set; }

        public string? CoverImage { get; set; }

        public string? Description { get; set; }

        public string? Genre { get; set; }

        public double Price { get; set; }

        public double? Revenue { get; set; }

        public required int DeveloperId { get; set; }

        public User? Developer { get; set; }

        public string? DownloadUrl { get; set; }

        public ICollection<Review>? Reviews { get; set; }
    }
}
