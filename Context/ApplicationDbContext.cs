using Microsoft.EntityFrameworkCore;
using TheOrchidArchade.Models;

namespace TheOrchidArchade.Context
{
    public class ApplicationDbContext: DbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
            : base(contextOptions) { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "Developer1", Email = "dev1@test.com",isDeveloper=true,revenue=0,Password="123" },
                new User { Id = 2, Username = "Developer2", Email = "dev2@test.com", isDeveloper = true, revenue = 0, Password = "123" },
                new User { Id = 3, Username = "Developer3", Email = "dev3@test.com", isDeveloper = true, revenue = 0, Password = "123" },
                new User { Id = 4, Username = "Client1", Email = "client1@test.com", isDeveloper = false, revenue = 0, Password = "123" },
                new User { Id = 5, Username = "Client2", Email = "client2@test.com", isDeveloper = false, revenue = 0, Password = "123" },
                new User { Id = 6, Username = "Client3", Email = "client3@test.com", isDeveloper = false, revenue = 0, Password = "123" }
            );

            modelBuilder.Entity<Game>().HasData(
                new Game { Id = 1, Title = "Leave", CoverImage = "https://i.imgur.com/8pVgqIR.png", Description = "You're a hacker alien trying to escape a giant ariship. Use your knowledge and your abilities to escape this misterious place.Use the arrows to move and z to open your console.", Genre = "Puzzles", Price = 10.3, Revenue=0, DeveloperId=1, DownloadUrl= "https://drive.google.com/file/d/13VCHbObQlezmir5Q4e2wFdb-pVE-LC_u/view?usp=drive_link" },
                new Game { Id = 2, Title = "Purification", CoverImage = "https://i.imgur.com/LWjv79J.png", Description = "The cascade, your home, has been invaded by monsters . Defeat the monsters and purify the cascade back to its normal state.", Genre = "Bullet hell", Price = 0, Revenue = 0, DeveloperId = 1, DownloadUrl = "https://drive.google.com/file/d/1kEO_qjonwWC4CIQxBDLAy2qfe1n1gl57/view?usp=drive_link" },
                new Game { Id = 3, Title = "TheAcumulator", CoverImage = "https://i.imgur.com/sWw7n5u.png", Description = "Explore a haunted house riddled with monsters, puzzles and secrets trying to unravel the mysteries of this place.", Genre = "Horror", Price = 5, Revenue = 0, DeveloperId = 1, DownloadUrl = "https://drive.google.com/file/d/1VJk1Vq4bvS6INPSuOSu2eF3uJmTxRdgo/view?usp=drive_link" },
                new Game { Id = 4, Title = "MonoCycle", CoverImage = "https://i.imgur.com/ikEFkww.png", Description = "You're a Monkey on a Monocycle", Genre = "Platformer", Price = 4, Revenue = 0, DeveloperId = 1, DownloadUrl = "https://drive.google.com/file/d/1bAt1OLeVvi3Dr5yF6eWzaFXSJLy_YJjM/view?usp=sharing" },
                new Game { Id = 5, Title = "Froggy Freeway", CoverImage = "https://i.imgur.com/QmpRPyE.png", Description = "The frogs got teleported away from their home. Grow or shrink the objects of the environment to help them get back.", Genre = "Puzzles", Price = 0, Revenue = 0, DeveloperId = 2, DownloadUrl = "https://enguenye.itch.io/froggyfreeway" },
                new Game { Id = 6, Title = "1098-Bee", CoverImage = "https://img.itch.zone/aW1nLzEzNzI4MTM2LnBuZw==/original/xZxaEf.png", Description = "The IRS Requisitioned all of your bees! Fight through hordes of government employees in an epic battle of the birds and the bees.", Genre = "Puzzles", Price = 15.6, Revenue = 0, DeveloperId = 2, DownloadUrl = "https://magicow.itch.io/1098-bee" }
             );

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction { Id = 1, UserId = 4, GameId = 1, Date = new DateTime() },
                new Transaction { Id = 2, UserId = 4, GameId = 2, Date = new DateTime() },
                new Transaction { Id = 3, UserId = 4, GameId = 3, Date = new DateTime() },
                new Transaction { Id = 4, UserId = 4, GameId = 4, Date = new DateTime() },
                new Transaction { Id = 5, UserId = 4, GameId = 5, Date = new DateTime() },
                new Transaction { Id = 6, UserId = 4, GameId = 6, Date = new DateTime() },
                new Transaction { Id = 7, UserId = 5, GameId = 3, Date = new DateTime() }
             );

            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1,Description="Awesome game, I loved the Alien",Rating=5, UserId = 4, GameId = 1},
                new Review { Id = 2, Description = "Pretty cool concept", Rating = 4, UserId = 4, GameId = 2 },
                new Review { Id = 3, Description = "Pretty terrifying", Rating = 5, UserId = 4, GameId = 3 },
                new Review { Id = 4, Description = "This game sucks!!!", Rating = 1, UserId = 4, GameId = 4 },
                new Review { Id = 5, Description = "Was not my cup of tea", Rating = 2, UserId = 5, GameId = 1 }
             );
        }

        public DbSet<User> users { get; set; }

        public DbSet<Review> reviews { get; set; }

        public DbSet<Game> games { get; set; }

        public DbSet<Transaction> transactions { get; set; }
    }
}
