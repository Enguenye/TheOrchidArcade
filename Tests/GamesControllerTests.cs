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
    public class GamesControllerTests
    {
        private readonly GamesController _controller;
        private readonly ApplicationDbContext _context;

        public GamesControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestGameDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new GamesController(_context);
        }

        [Fact]
        public async Task GetGameListTest()
        {
            var developer = new User { Id = 1, Username = "Developer1", Email= "Developer1@test.com", isDeveloper=true };
            var games = new List<Game>
            {
                new Game { Id = 1, Title = "Game1", Price = 2, DeveloperId = developer.Id },
                new Game { Id = 2, Title = "Game2", Price = 2, DeveloperId = developer.Id }
            };

            _context.users.Add(developer);
            _context.games.AddRange(games);
            await _context.SaveChangesAsync();

            var result = await _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Game>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task CreateGameTest()
        {
            var developer = new User { Id = 2, Username = "Developer2", Email = "Developer2@test.com", isDeveloper = true };
            _context.users.Add(developer);
            await _context.SaveChangesAsync();

            var newGame = new Game { Id = 3, Title = "NewGame", Price = 3, DeveloperId = developer.Id };

            var result = await _controller.Create(newGame);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var gameInDb = await _context.games.FindAsync(3);
            Assert.NotNull(gameInDb);
            Assert.Equal(newGame.Title, gameInDb.Title);
        }

        [Fact]
        public async Task UpdateGameTest()
        {
            var developer = new User { Id = 3, Username = "Developer3", Email = "Developer3@test.com", isDeveloper = true };
            var game = new Game { Id = 4, Title = "Game4", Price = 4, DeveloperId = developer.Id };
            _context.users.Add(developer);
            _context.games.Add(game);
            await _context.SaveChangesAsync();

            _context.Entry(game).State = EntityState.Detached;

            var updatedGame = await _context.games.FindAsync(game.Id);
            updatedGame.Title = "UpdatedGame4";
            updatedGame.Price = 5;

            var result = await _controller.Edit(updatedGame.Id, updatedGame);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var gameInDb = await _context.games.FindAsync(updatedGame.Id);
            Assert.NotNull(gameInDb);
            Assert.Equal("UpdatedGame4", gameInDb.Title);
            Assert.Equal(5, gameInDb.Price);
        }

        [Fact]
        public async Task DeleteGameTest()
        {
            var developer = new User { Id = 4, Username = "Developer4", Email = "Developer4@test.com", isDeveloper = true };
            var game = new Game { Id = 5, Title = "Game5", Price = 4, DeveloperId = developer.Id };
            _context.users.Add(developer);
            _context.games.Add(game);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteConfirmed(game.Id);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var gameInDb = await _context.games.FindAsync(game.Id);
            Assert.Null(gameInDb);
        }

        [Fact]
        public async Task BuyGameTest()
        {
            var developer = new User { Id = 5, Username = "Developer5", Email = "Developer5@test.com", isDeveloper = true };
            var game = new Game { Id = 6, Title = "Game6", Price = 4, DeveloperId = developer.Id };
            var user = new User { Id = 6, Username = "testuser", Email = "testuser@test.com", isDeveloper = false };
            _context.users.AddRange(developer, user);
            _context.games.Add(game);
            await _context.SaveChangesAsync();

            await _controller.BuyGame(user.Id, game.Id);

            var transactionInDb = await _context.transactions.FirstOrDefaultAsync(t => t.UserId == user.Id && t.GameId == game.Id);
            Assert.NotNull(transactionInDb);
        }
    }
}
