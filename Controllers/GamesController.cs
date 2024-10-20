using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.games.Include(g => g.Developer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.games
                .Include(g => g.Developer)
                .Include(g => g.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            var developers = _context.users
                 .Where(u => u.isDeveloper == true)
                 .Select(u => new { u.Id, u.Username })
                 .ToList();
            ViewData["DeveloperId"] = new SelectList(developers, "Id", "Username");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CoverImage,Description,Genre,Price,Revenue,DownloadUrl,DeveloperId")] Game game)
        {
            game.Revenue = 0;
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(_context.users, "Id", "Id", game.DeveloperId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CoverImage,Description,Genre,Price,Revenue,DownloadUrl,DeveloperId")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperId"] = new SelectList(_context.users, "Id", "Id", game.DeveloperId);
            return View(game);
        }

        public IActionResult Buy(int id)
        {
            var game = _context.games.Find(id);

            if (game == null)
            {
                return NotFound();
            }
            var viewModel = new BuyGameViewModel
            {
                GameId = game.Id,
                Title = game.Title,
                Price = game.Price
            };

            ViewBag.UserId = new SelectList(_context.users, "Id", "Username");

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyGame(int userId, int gameId)
        {

            var transaction = new Transaction
            {
                UserId = userId,
                GameId = gameId,
                Date = DateTime.Now
            };


            if (ModelState.IsValid)
            {
                _context.transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Users", new { id = userId });
            }

            return View();
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.games
                .Include(g => g.Developer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.games.FindAsync(id);
            if (game != null)
            {
                _context.games.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.games.Any(e => e.Id == id);
        }
    }
}
