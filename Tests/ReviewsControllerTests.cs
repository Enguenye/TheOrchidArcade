using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheOrchidArchade.Controllers;
using TheOrchidArchade.Models;
using TheOrchidArchade.Context;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TheOrchidArchade.Tests
{
    public class ReviewsControllerTests
    {
        private readonly ReviewsController _controller;
        private readonly ApplicationDbContext _context;

        public ReviewsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestReviewDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new ReviewsController(_context);
        }

        [Fact]
        public async Task ViewReviewListTest()
        {
            var user = new User { Id = 1, Username = "testuser", Email = "testuser@test.com", isDeveloper = true };
            var game = new Game { Id = 1, Title = "Test Game", Price = 5, DeveloperId = 1 };
            var reviews = new List<Review>
            {
                new Review { Id = 1, Description = "Great game!", Rating = 5, GameId = game.Id, UserId = user.Id },
                new Review { Id = 2, Description = "Not bad", Rating = 4, GameId = game.Id, UserId = user.Id }
            };

            _context.users.Add(user);
            _context.games.Add(game);
            _context.reviews.AddRange(reviews);
            await _context.SaveChangesAsync();

            var result = await _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Review>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task CreateReviewTest()
        {
            var user = new User { Id = 2, Username = "reviewer", Email = "reviewer@test.com", isDeveloper = false };
            var game = new Game { Id = 2, Title = "Another Game", Price = 5, DeveloperId = 2 };

            _context.users.Add(user);
            _context.games.Add(game);
            await _context.SaveChangesAsync();

            var newReview = new Review
            {
                Id = 3,
                Description = "Amazing gameplay",
                Rating = 5,
                GameId = game.Id,
                UserId = user.Id
            };

            var result = await _controller.Create(newReview);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var reviewInDb = await _context.reviews.FindAsync(3);
            Assert.NotNull(reviewInDb);
            Assert.Equal(newReview.Description, reviewInDb.Description);
        }

        [Fact]
        public async Task UpdateReviewTest()
        {
            var user = new User { Id = 3, Username = "user3", Email = "user3@test.com", isDeveloper = true };
            var game = new Game { Id = 3, Title = "Yet Another Game", Price = 5, DeveloperId = 3 };

            var review = new Review
            {
                Id = 4,
                Description = "Good but could be better",
                Rating = 3,
                GameId = game.Id,
                UserId = user.Id
            };

            _context.users.Add(user);
            _context.games.Add(game);
            _context.reviews.Add(review);
            await _context.SaveChangesAsync();

            _context.Entry(review).State = EntityState.Detached; // Detach the tracked entity

            var updatedReview = await _context.reviews.FindAsync(review.Id);
            updatedReview.Description = "Better after updates";
            updatedReview.Rating = 4;

            var result = await _controller.Edit(updatedReview.Id, updatedReview);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var reviewInDb = await _context.reviews.FindAsync(updatedReview.Id);
            Assert.NotNull(reviewInDb);
            Assert.Equal("Better after updates", reviewInDb.Description);
            Assert.Equal(4, reviewInDb.Rating);
        }
    }
}
