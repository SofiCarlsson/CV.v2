using CV_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CV_v2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserContext users;

        public HomeController(ILogger<HomeController> logger, UserContext service)
        {
            _logger = logger;
            users = service;
        }

        public IActionResult Index()
        {
            // Ladda användare och deras tillhörande CV
            var usersList = users.Users
                .Include(u => u.CV) // Inkludera relaterade CV-objekt
                .ToList();

            return View(usersList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}