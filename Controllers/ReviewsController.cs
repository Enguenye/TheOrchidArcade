using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheOrchidArchade.Context;
using TheOrchidArchade.Models;

namespace TheOrchidArchade.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(ApplicationDbContext context, ILogger<ReviewsController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.reviews
                .Include(r => r.Game)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        [HttpGet("Review/Create")]
        [Authorize]
        public IActionResult Create(string gameId, string userId)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserId == null || authenticatedUserId != userId)
            {
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to create a review for this user" });
            }
            var review = new Review
            {
                GameId = gameId,
                UserId = userId
            };
            return View(review);
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Review/Create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Description,Rating,GameId,UserId")] Review review)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserId == null || authenticatedUserId != review.UserId)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + review.UserId);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to create a review for this user" });
            }

            var existingReview = await _context.reviews
        .FirstOrDefaultAsync(r => r.GameId == review.GameId && r.UserId == review.UserId);

            if (existingReview != null)
            {
                _logger.LogError("User "+ authenticatedUserId + " tried to create a review of game"+ review.GameId + " that already had a review");
                return RedirectToAction("Error", "Home", new { message = "You have already reviewed this game." });
            }

            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User " + authenticatedUserId + "created review for game " + review.GameId);
                return RedirectToAction("Details", "Games", new { id = review.GameId });
            }
            return View(review);
        }

        // GET: Reviews/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " Review accessed  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit a review for this user" });
            }

            var review = await _context.reviews.FindAsync(id);
            if (review == null)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " Review accessed  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit a review for this user" });
            }

            
            if (authenticatedUserId == null || authenticatedUserId != review.UserId)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " Review accessed  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit a review for this user" });
            }

            ViewData["GameId"] = new SelectList(_context.games, "Id", "Id", review.GameId);
            ViewData["UserId"] = new SelectList(_context.users, "Id", "Id", review.UserId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Description,Rating")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingReview = await _context.reviews
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == authenticatedUserId);

            if (existingReview == null)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " Review accessed  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this review or it doesn't exist" });
            }

                try
                {
                    existingReview.Description = review.Description;
                    existingReview.Rating = review.Rating;
                    _logger.LogInformation("Updated review with id: " + review.Id);
                    _context.Update(existingReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError("Error updating review: " + ex.Message);
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Users", new { id = existingReview.UserId });
        }

        // GET: Reviews/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.reviews
                .Include(r => r.Game)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserId == null || authenticatedUserId != review.UserId)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " Review accessed  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit a review for this user" });
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var review = await _context.reviews.FindAsync(id);
            if (review != null)
            {
                var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (authenticatedUserId == null || authenticatedUserId != review.UserId)
                {
                    _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " Review accessed  " + id);
                    return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit a review for this user" });
                }
                _logger.LogInformation("Deleting review with id: " + review.Id);
                _context.reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Users", new { id = review.UserId });
        }

        private bool ReviewExists(string id)
        {
            return _context.reviews.Any(e => e.Id == id);
        }
    }
}
