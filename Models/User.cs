using Microsoft.AspNetCore.Identity;

namespace TheOrchidArchade.Models
{
    public class User : IdentityUser
    {
        public double? revenue { get; set; }

        public required bool isDeveloper { get; set; }

        public string? creditCardNumber { get; set; }

        public ICollection<Review>? Reviews { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
