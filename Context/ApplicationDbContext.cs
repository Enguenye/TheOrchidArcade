using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheOrchidArchade.Models;

namespace TheOrchidArchade.Context
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
            : base(contextOptions) { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var hasher = new PasswordHasher<User>();

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
    .HasOne(g => g.Developer)
    .WithMany()
    .HasForeignKey(g => g.DeveloperId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Game)
                .WithMany(g => g.Transactions)
                .HasForeignKey(t => t.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            var passhash = "AQAAAAIAAYagAAAAEGZ6Z2QZrR0CXOLW8i95LxiUw2kOGmEmVcPWMN31PUmZFacT7AEg3QdKvZtPsEPTrw==";//Corresponds to BlaBla-123 PasswordHash

            string idUser1 = Guid.NewGuid().ToString();
            string idUser2 = Guid.NewGuid().ToString();
            string idUser3 = Guid.NewGuid().ToString();
            string idUser4 = Guid.NewGuid().ToString();
            string idUser5 = Guid.NewGuid().ToString();
            string idUser6 = Guid.NewGuid().ToString();

            string idGame1 = Guid.NewGuid().ToString();
            string idGame2 = Guid.NewGuid().ToString();
            string idGame3 = Guid.NewGuid().ToString();
            string idGame4 = Guid.NewGuid().ToString();
            string idGame5 = Guid.NewGuid().ToString();
            string idGame6 = Guid.NewGuid().ToString();

            modelBuilder.Entity<User>().HasData(
                new User { Id = idUser1, UserName = "Developer1", Email = "dev1@test.com",isDeveloper=true,revenue=0 },
                new User { Id = idUser2, UserName = "Developer2", Email = "dev2@test.com", isDeveloper = true, revenue = 0, PasswordHash = passhash },
                new User { Id = idUser3, UserName = "Developer3", Email = "dev3@test.com", isDeveloper = true, revenue = 0, PasswordHash = passhash },
                new User { Id = idUser4, UserName = "Client1", Email = "client1@test.com", isDeveloper = false, revenue = 0, PasswordHash = passhash },
                new User { Id = idUser5, UserName = "Client2", Email = "client2@test.com", isDeveloper = false, revenue = 0, PasswordHash = passhash },
                new User { Id = idUser6, UserName = "Client3", Email = "client3@test.com", isDeveloper = false, revenue = 0, PasswordHash = passhash }
            );

            modelBuilder.Entity<Game>().HasData(
                new Game { Id = idGame1, Title = "Leave", CoverImage = "https://i.imgur.com/8pVgqIR.png", Description = "You're a hacker alien trying to escape a giant ariship. Use your knowledge and your abilities to escape this misterious place.Use the arrows to move and z to open your console.", Genre = "Puzzles", Price = 10.3, Revenue=0, DeveloperId= idUser1, DownloadUrl= "https://drive.google.com/file/d/13VCHbObQlezmir5Q4e2wFdb-pVE-LC_u/view?usp=drive_link" },
                new Game { Id = idGame2, Title = "Purification", CoverImage = "https://i.imgur.com/LWjv79J.png", Description = "The cascade, your home, has been invaded by monsters . Defeat the monsters and purify the cascade back to its normal state.", Genre = "Bullet hell", Price = 0, Revenue = 0, DeveloperId = idUser1, DownloadUrl = "https://drive.google.com/file/d/1kEO_qjonwWC4CIQxBDLAy2qfe1n1gl57/view?usp=drive_link" },
                new Game { Id = idGame3, Title = "TheAcumulator", CoverImage = "https://i.imgur.com/sWw7n5u.png", Description = "Explore a haunted house riddled with monsters, puzzles and secrets trying to unravel the mysteries of this place.", Genre = "Horror", Price = 5, Revenue = 0, DeveloperId = idUser1, DownloadUrl = "https://drive.google.com/file/d/1VJk1Vq4bvS6INPSuOSu2eF3uJmTxRdgo/view?usp=drive_link" },
                new Game { Id = idGame4, Title = "MonoCycle", CoverImage = "https://i.imgur.com/ikEFkww.png", Description = "You're a Monkey on a Monocycle", Genre = "Platformer", Price = 4, Revenue = 0, DeveloperId = idUser1, DownloadUrl = "https://drive.google.com/file/d/1bAt1OLeVvi3Dr5yF6eWzaFXSJLy_YJjM/view?usp=sharing" },
                new Game { Id = idGame5, Title = "Froggy Freeway", CoverImage = "https://i.imgur.com/QmpRPyE.png", Description = "The frogs got teleported away from their home. Grow or shrink the objects of the environment to help them get back.", Genre = "Puzzles", Price = 0, Revenue = 0, DeveloperId = idUser2, DownloadUrl = "https://enguenye.itch.io/froggyfreeway" },
                new Game { Id = idGame6, Title = "1098-Bee", CoverImage = "https://img.itch.zone/aW1nLzEzNzI4MTM2LnBuZw==/original/xZxaEf.png", Description = "The IRS Requisitioned all of your bees! Fight through hordes of government employees in an epic battle of the birds and the bees.", Genre = "Puzzles", Price = 15.6, Revenue = 0, DeveloperId = idUser2, DownloadUrl = "https://magicow.itch.io/1098-bee" }
             );

            modelBuilder.Entity<Transaction>().HasData(
                new Transaction { Id = Guid.NewGuid().ToString(), UserId = idUser4, GameId = idGame1, Date = new DateTime() },
                new Transaction { Id = Guid.NewGuid().ToString(), UserId = idUser4, GameId = idGame2, Date = new DateTime() },
                new Transaction { Id = Guid.NewGuid().ToString(), UserId = idUser4, GameId = idGame3, Date = new DateTime() },
                new Transaction { Id = Guid.NewGuid().ToString(), UserId = idUser4, GameId = idGame4, Date = new DateTime() },
                new Transaction { Id = Guid.NewGuid().ToString(), UserId = idUser4, GameId = idGame5, Date = new DateTime() },
                new Transaction { Id = Guid.NewGuid().ToString(), UserId = idUser4, GameId = idGame6, Date = new DateTime() },
                new Transaction { Id = Guid.NewGuid().ToString(), UserId = idUser5, GameId = idGame3, Date = new DateTime() }
             );

            modelBuilder.Entity<Review>().HasData(
                new Review { Id = Guid.NewGuid().ToString(), Description="Awesome game, I loved the Alien",Rating=5, UserId = idUser4, GameId = idGame1 },
                new Review { Id = Guid.NewGuid().ToString(), Description = "Pretty cool concept", Rating = 4, UserId = idUser4, GameId = idGame2 },
                new Review { Id = Guid.NewGuid().ToString(), Description = "Pretty terrifying", Rating = 5, UserId = idUser4, GameId = idGame3 },
                new Review { Id = Guid.NewGuid().ToString(), Description = "This game sucks!!!", Rating = 1, UserId = idUser4, GameId = idGame4 },
                new Review { Id = Guid.NewGuid().ToString(), Description = "Was not my cup of tea", Rating = 2, UserId = idUser5, GameId = idGame1 }
             );

            
        }

        public DbSet<User> users { get; set; }

        public DbSet<Review> reviews { get; set; }

        public DbSet<Game> games { get; set; }

        public DbSet<Transaction> transactions { get; set; }
    }
}
