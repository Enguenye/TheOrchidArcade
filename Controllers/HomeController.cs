using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TheOrchidArchade.Models;

namespace TheOrchidArchade.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Account()
        {
            return View();
        }

    }
}
