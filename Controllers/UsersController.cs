using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TheOrchidArchade.Context;
using TheOrchidArchade.Models;
using TheOrchidArchade.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TheOrchidArchade.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly SignInManager<User> _signInManager;

        private readonly UserManager<User> _userManager;

        private readonly ILogger<UsersController> _logger;

        private readonly EncryptionHelper _encryptionHelper;

        public UsersController(ApplicationDbContext context, SignInManager<User> signInManager, UserManager<User> userManager, ILogger<UsersController> logger, EncryptionHelper encryptionHelper)
        {
            _context = context;

            _signInManager = signInManager;

            _userManager = userManager;

            _logger = logger;
            _encryptionHelper = encryptionHelper;
        }


        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserId == null || authenticatedUserId != id)
            {
                _logger.LogError("Tried to access user without authorization");
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to view this user" });
            }

            var user = await _context.users.FindAsync(id);

            if (user == null)
            {
                _logger.LogError("Tried to access details of non existent user");
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to view this user" });
            }

            // Query to get all games for this user via the Transactions table
            var userGames = from t in _context.transactions
                            join g in _context.games on t.GameId equals g.Id
                            where t.UserId == id
                            select g;

            var createdGames = from g in _context.games
                               where g.DeveloperId == id
                               select g;

            var userReviews = await _context.reviews
                              .Include(r => r.Game) 
                              .Where(r => r.UserId == id)
                              .ToListAsync();
            user.creditCardNumber = _encryptionHelper.Decrypt(user.creditCardNumber);

            var viewModel = new UserDetailsViewModel
            {
                User = user,
                Games = await userGames.ToListAsync(),
                CreatedGames=await createdGames.ToListAsync(),
                Reviews = userReviews
            };

            return View(viewModel);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,UserName,revenue,isDeveloper,creditCardNumber")] User user, string password)
        {
            user.revenue = 0;

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(user.creditCardNumber))
                {
                    ModelState.AddModelError("", "You need to add a credit card number");
                    return View(user);
                }
                user.creditCardNumber= _encryptionHelper.Encrypt(user.creditCardNumber);
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("New user created with user Username: " + user.UserName);
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
                    try
                    {
                        var emailSent = EmailSender.SendEmail(user.Email, $"Your 2FA code is: {token}", _logger);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Failed to send 2FA code: {ex.Message}");
                        ModelState.AddModelError("", "Failed to send 2FA code. Please try again.");
                        return View(user);
                    }

                    return RedirectToAction("Verify2FA", new { userId = user.Id });
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Failed to create user: {error.Description}");
                    ModelState.AddModelError("", "Failed to create user " + error.Description);
                    return View(user);
                }
            }

            return View(user);
        }

        public IActionResult Verify2FA(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError($"Tried to authenticate with non existent user: " + userId);
                return RedirectToAction("Error", "Home", new { message = "User cannot be null" });
            }

            ViewData["UserId"] = userId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Verify2FA(Verify2FAViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                _logger.LogError($"Tried to authenticate with non existent user: " + model.UserId);
                ModelState.AddModelError("", "Invalid authentication attempt.");
                return RedirectToAction("Login");
            }

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, model.Code);
            if (isValid)
            {
                user.EmailConfirmed = true;
                user.TwoFactorEnabled = true;
                await _userManager.UpdateAsync(user);
                await _signInManager.SignInAsync(user, isPersistent: true);
                HttpContext.Session.SetString("IsDeveloper", user.isDeveloper.ToString());
                return RedirectToAction("Details", new { id = user.Id });
            }

            ModelState.AddModelError("", "Invalid authentication code.");
            return View(model);
        }

        public IActionResult Success(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError($"Tried to authenticate with non existent user: " + userId);
                return RedirectToAction("Error", "Home", new { message = "Wrong input" });
            }
            _logger.LogInformation($"User " + userId + " was authenticated");
            ViewData["UserId"] = userId;
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserId == null || authenticatedUserId != id)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this user" });
            }
            if (id == null)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this user" });
            }

            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this user" });
            }
            user.creditCardNumber = _encryptionHelper.Decrypt(user.creditCardNumber);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,UserName,revenue,creditCardNumber")] User user, string password)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserId == null || authenticatedUserId != id)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this user" });
            }

            if (id != user.Id)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this user" });
            }

                try
                {
                    var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
                    if (existingUser == null)
                    {
                        _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                        return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this user" });
                    }
                    existingUser.Email = user.Email;
                    existingUser.UserName = user.UserName;
                    existingUser.creditCardNumber = _encryptionHelper.Encrypt(user.creditCardNumber);    

                    if (!string.IsNullOrEmpty(password))
                    {
                        var removePasswordResult = await _userManager.RemovePasswordAsync(existingUser);
                        if (removePasswordResult.Succeeded)
                        {
                            var addPasswordResult = await _userManager.AddPasswordAsync(existingUser, password);
                            if (!addPasswordResult.Succeeded)
                            {
                                foreach (var error in addPasswordResult.Errors)
                                {
                                    _logger.LogError($"Error changing user password: " + error.Description);
                                    return RedirectToAction("Error", "Home", new { message = error.Description });
                                }
                            }
                        }
                        else
                        {
                            foreach (var error in removePasswordResult.Errors)
                            {
                                _logger.LogError($"Error changing user password: " + error.Description);
                                return RedirectToAction("Error", "Home", new { message = "Error updating password" });
                            }
                            return View(user);
                        }
                    }

                    var updateResult = await _userManager.UpdateAsync(existingUser);
                    if (updateResult.Succeeded)
                    {
                         _logger.LogInformation($"Updated user with user id:" + user.Id);
                         return RedirectToAction("Details", new { id = user.Id });
                    }
                    else
                    {
                        foreach (var error in updateResult.Errors)
                        {
                            _logger.LogError($"Error updating user: " + error.Description);
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Error updating user: " + ex.Message);
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
        
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "This account is locked out. Please try again later.");
                        return View(model);
                    }
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,true);
                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
                        var emailSent = EmailSender.SendEmail(user.Email, $"Your login 2FA code is: {token}",_logger);
                        if (!emailSent)
                        {
                            _logger.LogError($"Failed to send 2FA email to email: " + user.Email);
                            ModelState.AddModelError("", "Failed to send 2FA email. Please try again.");
                            return View(model);
                        }
                        return RedirectToAction("Verify2FA", new { userId = user.Id });
                    }
                }

                ModelState.AddModelError("", "Invalid login attempt.");
            }

            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Delete(string? id)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserId == null || authenticatedUserId != id)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this user" });
            }

            if (id == null)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this user" });
            }

            var user = await _context.users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this user" });
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authenticatedUserId == null || authenticatedUserId != id)
            {
                _logger.LogError($"Tried to authenticate with wrong user. Current authenticated: " + authenticatedUserId + " User authenticated:  " + id);
                return RedirectToAction("Error", "Home", new { message = "You don't have permission to edit this user" });
            }
            var user = await _context.users.FindAsync(id);
            if (user != null)
            {
                _logger.LogInformation($"Removing user with id: " + id);
                _context.users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Games", new { message = "User was deleted" });
            }

            
            _logger.LogError($"Error deleting user with id: " + id);
            return RedirectToAction("Error", "Home", new { message = "There was an error procesing your request" });
        }

        private bool UserExists(string id)
        {
            return _context.users.Any(e => e.Id == id);
        }
    }
}
