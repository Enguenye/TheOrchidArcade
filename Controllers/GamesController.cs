using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheOrchidArchade.Context;
using TheOrchidArchade.Models;

namespace TheOrchidArchade.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<GamesController> _logger;

        public GamesController(ApplicationDbContext context, ILogger<GamesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.games.Include(g => g.Developer);

            string isDeveloperSession = "";
            if(HttpContext!=null)
                isDeveloperSession = HttpContext.Session.GetString("IsDeveloper");

            bool isDeveloper = !string.IsNullOrEmpty(isDeveloperSession) && isDeveloperSession == "True";

            ViewData["IsDeveloper"] = isDeveloper;

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.games
                .Include(g => g.Developer)
                .Include(g => g.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,CoverImage,Description,Genre,Price,Revenue,DownloadUrl")] Game game)
        {
            game.Revenue = 0;
            game.DeveloperId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Creating game: " + game.Title + "by developer " + game.DeveloperId);
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Games/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this game" });
            }

            var game = await _context.games.FindAsync(id);
            if (game == null)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this game" });
            }

            if (game.DeveloperId != authenticatedUserId)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this game" });
            }

            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,CoverImage,Description,Genre,Price,DownloadUrl")] Game updatedGame)
        {
            if (id != updatedGame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingGame = await _context.games.FindAsync(id);
                    if (existingGame == null)
                    {
                        _logger.LogError($"Tried to edit non existing game with id: " + id);
                        return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this game" });
                    }

                    var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (existingGame.DeveloperId != authenticatedUserId)
                    {
                        _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                        return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this game" });
                    }

                    existingGame.Title = updatedGame.Title;
                    existingGame.CoverImage = updatedGame.CoverImage;
                    existingGame.Description = updatedGame.Description;
                    existingGame.Genre = updatedGame.Genre;
                    existingGame.Price = updatedGame.Price;
                    existingGame.DownloadUrl = updatedGame.DownloadUrl;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Error editing game: " + ex.Message);
                    if (!GameExists(updatedGame.Id))
                    {      
                        return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this game" });
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedGame);
        }


        [Authorize]
        public IActionResult Buy(string id)
        {
            var game = _context.games.Find(id);

            if (game == null)
            {
                _logger.LogError($"Tried to buy non existing game with id: " + id);
                return NotFound();
            }

            var viewModel = new BuyGameViewModel
            {
                GameId = game.Id,
                Title = game.Title,
                Price = game.Price
            };

            ViewBag.UserId = new SelectList(_context.users, "Id", "UserName");

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(BuyGameViewModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError($"Tried to buy game without authorization");
                return RedirectToAction("Error", "Home", new { message = "Please log in first" });
            }

            var existingTransaction = await _context.transactions
        .FirstOrDefaultAsync(t => t.UserId == userId && t.GameId == model.GameId);

            if (existingTransaction != null)
            {
                return RedirectToAction("Error", "Home", new { message = "You already own this game." });
            }

            var game = await _context.games.FindAsync(model.GameId);
            if (game == null)
            {
                _logger.LogError($"Tried to buy non existing game with id: " + model.GameId);
                return RedirectToAction("Error", "Home", new { message = "Game not found" });
            }

            game.Revenue += game.Price;

            var transaction = new Transaction
            {
                UserId = userId,
                GameId = model.GameId,
                Date = DateTime.Now
            };
            _context.transactions.Add(transaction);

            

            await _context.SaveChangesAsync();
            _logger.LogInformation($"User with id " + userId + " bought game with id: " + model.GameId);

            return RedirectToAction("Details", "Users", new { id = userId });

        }

        // GET: Games/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string? id)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this game" });
            }

            var game = await _context.games
                .Include(g => g.Developer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                _logger.LogError($"Tried to delete non existent game with id: " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this game" });
            }

            

            if (game.DeveloperId != authenticatedUserId)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this game" });
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var game = await _context.games.FindAsync(id);
            if (game != null)
            {
                var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (game.DeveloperId != authenticatedUserId)
                {
                    _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                    return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this game" });
                }
                _logger.LogInformation($"Deleting game with id: " + id);
                _context.games.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(string id)
        {
            return _context.games.Any(e => e.Id == id);
        }
    }
}
