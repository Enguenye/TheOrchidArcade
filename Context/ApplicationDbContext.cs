using Microsoft.EntityFrameworkCore;
using TheOrchidArchade.Models;

namespace TheOrchidArchade.Context
{
    public class ApplicationDbContext: DbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
            : base(contextOptions) { 
        
        }

        public DbSet<User> users { get; set; }

        public DbSet<Review> reviews { get; set; }

        public DbSet<Game> games { get; set; }

        public DbSet<Transaction> transactions { get; set; }
    }
}
