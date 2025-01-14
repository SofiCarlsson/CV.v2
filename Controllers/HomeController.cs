using CV_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CV_v2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserContext _context;

        public HomeController(ILogger<HomeController> logger, UserContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string firstname)
        {
            // Hämta alla CV:n och projekt
            var cvs = _context.CVs
                .Include(cv => cv.User)
                .Include(cv => cv.Competences)
                .ThenInclude(c => c.Competences)
                .Include(cv => cv.Educations)
                .ThenInclude(e => e.Education)
                .Include(cv => cv.WorkExperiences)
                .ThenInclude(w => w.WorkExperience)
                .AsQueryable();

            // Filtrera om förnamn är angivet
            if (!string.IsNullOrWhiteSpace(firstname))
            {
                firstname = firstname.ToLower(); // Gör input till gemener
                cvs = cvs.Where(cv => cv.User.Firstname.ToLower().StartsWith(firstname)); // Kontrollera om namnet börjar med bokstaven
            }

            var projects = await _context.Projects.ToListAsync();

            // Skapa modellen för vyn
            var startPageViewModel = new StartPageViewModel
            {
                Cvs = await cvs.ToListAsync(),
                Projects = projects
            };

            // Skicka tillbaka sökparametern för att återfylla sökfältet
            ViewData["Firstname"] = firstname;

            return View(startPageViewModel);
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
