using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheOrchidArchade.Controllers;
using TheOrchidArchade.Models;
using TheOrchidArchade.Context;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using System.Security.Claims;

namespace TheOrchidArchade.Tests
{
    public class ReviewsControllerTests
    {
        private readonly ReviewsController _controller;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestReviewDatabase")
                .Options;

            var mockLogger = new Mock<ILogger<ReviewsController>>();
            _logger = mockLogger.Object;

            _context = new ApplicationDbContext(options);
            _controller = new ReviewsController(_context, _logger);
        }



        [Fact]
        public async Task CreateReviewTest()
        {
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "reviewer", Email = "reviewer@test.com", isDeveloper = false };
            var game = new Game { Id = Guid.NewGuid().ToString(), Title = "Another Game", Price = 5, DeveloperId = user.Id };

            _context.users.Add(user);
            _context.games.Add(game);
            await _context.SaveChangesAsync();

            var newReview = new Review
            {
                Id = Guid.NewGuid().ToString(),
                Description = "Amazing gameplay",
                Rating = 5,
                GameId = game.Id,
                UserId = user.Id
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            await _controller.Create(newReview);

            var reviewInDb = await _context.reviews.FindAsync(newReview.Id);
            Assert.NotNull(reviewInDb);
            Assert.Equal(newReview.Description, reviewInDb.Description);
        }

        [Fact]
        public async Task UpdateReviewTest()
        {
            var user = new User { Id = Guid.NewGuid().ToString(), UserName = "user3", Email = "user3@test.com", isDeveloper = true };
            var game = new Game { Id = Guid.NewGuid().ToString(), Title = "Yet Another Game", Price = 5, DeveloperId = user.Id };

            var review = new Review
            {
                Id = Guid.NewGuid().ToString(),
                Description = "Good but could be better",
                Rating = 3,
                GameId = game.Id,
                UserId = user.Id
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            _context.users.Add(user);
            _context.games.Add(game);
            _context.reviews.Add(review);
            await _context.SaveChangesAsync();

            _context.Entry(review).State = EntityState.Detached;

            var updatedReview = await _context.reviews.FindAsync(review.Id);
            updatedReview.Description = "Better after updates";
            updatedReview.Rating = 4;

            var result = await _controller.Edit(updatedReview.Id, updatedReview);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            var reviewInDb = await _context.reviews.FindAsync(updatedReview.Id);
            Assert.NotNull(reviewInDb);
            Assert.Equal("Better after updates", reviewInDb.Description);
            Assert.Equal(4, reviewInDb.Rating);
        }
    }
}
