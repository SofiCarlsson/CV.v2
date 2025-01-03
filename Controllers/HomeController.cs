using CV_v2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace CV_v2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserContext users;

        public HomeController(ILogger<HomeController> logger, UserContext service)
        {
            _logger = logger;
            users = service;
        }

        public IActionResult Index()
        {
            // H�mta anv�ndarna fr�n databasen och konvertera till en lista
            var usersList = users.Users?.ToList(); // Anv�nd null-s�ker operator f�r att undvika null-referensfel

            // Om usersList �r null, skapa en tom lista
            if (usersList == null)
            {
                usersList = new List<User>(); // Skapa en tom lista om det �r null
            }

            // Skicka den icke-null listan till vyn
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



