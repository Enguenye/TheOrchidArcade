using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheOrchidArchade.Controllers;
using TheOrchidArchade.Models;
using TheOrchidArchade.Context;
using Xunit;

namespace TheOrchidArchade.Tests
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        private readonly ApplicationDbContext _context;

        public UsersControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Create an in-memory database
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new UsersController(_context);
        }

        [Fact]
        public async Task Index_ReturnsViewWithUserList()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Email = "test1@example.com", Username = "testuser1" },
                new User { Id = 2, Email = "test2@example.com", Username = "testuser2" }
            };

            // Add test users to the in-memory database
            _context.users.AddRange(users);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Create_AddsUserAndRedirects()
        {
            // Arrange
            var newUser = new User { Id = 1, Email = "test@example.com", Username = "testuser" };

            // Act
            var result = await _controller.Create(newUser);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectResult.ActionName);
            Assert.Equal(1, redirectResult.RouteValues["id"]);

            // Verify user was added to the in-memory database
            var userInDb = await _context.users.FindAsync(1);
            Assert.NotNull(userInDb);
            Assert.Equal(newUser.Email, userInDb.Email);
        }

    }
}
