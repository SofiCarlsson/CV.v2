using CV_v2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace CV_v2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserContext _context;

        public HomeController(ILogger<HomeController> logger, UserContext service)
        {
            _logger = logger;
            _context = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index() { 

            var cvs = await _context.CVs.ToListAsync();
            var projects = await _context.Projects.ToListAsync();

            var startPageViewModel = new StartPageViewModel
            {
                Cvs = cvs,
                Projects = projects
            };

            return View(startPageViewModel);
        }
        //public async Task<IActionResult> Shared()
        //{
        //    // Ladda användare och deras tillhörande CV
        //    var usersList = await users.Users
        //        .Include(u => u.CV) // Inkludera relaterade CV-objekt
        //        .ToListAsync();

        //    return View(usersList);
        //}

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