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
        public async Task GetUserListTest()
        {
            var users = new List<User>
            {
                new User { Id = 1, Email = "test1@example.com", Username = "testuser1",isDeveloper = false },
                new User { Id = 2, Email = "test2@example.com", Username = "testuser2",isDeveloper = true}
            };

            _context.users.AddRange(users);
            await _context.SaveChangesAsync();
            var result = await _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task CreateUserTest()
        {
            var newUser = new User { Id = 3, Email = "test3@example.com", Username = "testuser3", isDeveloper = true };
            var result = await _controller.Create(newUser);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectResult.ActionName);
            Assert.Equal(3, redirectResult.RouteValues["id"]);

            var userInDb = await _context.users.FindAsync(3);
            Assert.NotNull(userInDb);
            Assert.Equal(newUser.Email, userInDb.Email);
        }

        [Fact]
        public async Task DeleteUserTest()
        {
  
            var user = new User { Id = 4, Email = "test4@example.com", Username = "testuser4", isDeveloper = false };
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteConfirmed(user.Id);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var userInDb = await _context.users.FindAsync(user.Id);
            Assert.Null(userInDb); 
        }

        [Fact]
        public async Task UpdateUserTest()
        {

            var user = new User { Id = 5, Email = "test5@example.com", Username = "testuser5", isDeveloper = false };
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            _context.Entry(user).State = EntityState.Detached;

            var updatedUser = await _context.users.FindAsync(user.Id);
            updatedUser.Email = "updated5@example.com";
            updatedUser.Username = "updateduser5";
            updatedUser.isDeveloper = true;

            var result = await _controller.Edit(updatedUser.Id, updatedUser);

            var userInDb = await _context.users.FindAsync(updatedUser.Id);
            Assert.NotNull(userInDb);
            Assert.Equal("updated5@example.com", userInDb.Email); 
            Assert.Equal("updateduser5", userInDb.Username); 
            Assert.Equal(true, userInDb.isDeveloper); 
        }

    }
}
