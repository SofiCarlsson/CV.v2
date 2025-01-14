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


        //Metod för att sortera förstasidan
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

            var projects = _context.Projects
                .Include(p => p.User) 
                .Include(p => p.UsersInProject)
                .ThenInclude(up => up.User)
                .AsQueryable();

            // Om användaren inte är inloggad, filtrera bort privata profiler
            if (!User.Identity.IsAuthenticated)
            {
                cvs = cvs.Where(cv => !cv.User.IsProfilePrivate); 
                projects = projects.Where(p => p.User != null && !p.User.IsProfilePrivate); 
            }

            // Filtrera på förnamn om angivet
            if (!string.IsNullOrWhiteSpace(firstname))
            {
                firstname = firstname.ToLower();
                cvs = cvs.Where(cv => cv.User.Firstname.ToLower().StartsWith(firstname));

                // Filtrera projekt baserat på skaparen
                projects = projects.Where(p => p.User != null && p.User.Firstname.ToLower().StartsWith(firstname));
            }

            var startPageViewModel = new StartPageViewModel
            {
                Cvs = await cvs.ToListAsync(),
                Projects = await projects.ToListAsync()
            };

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
