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


        //Metod för att hålla koll på om Användaren är privat.
        private IEnumerable<UserInProject> FilterPrivateProfiles(IEnumerable<UserInProject> usersInProject, bool isAuthenticated)
        {
            if (!isAuthenticated)
            {
                // Filtrera bort deltagare med privata profiler om användaren inte är inloggad
                return usersInProject.Where(up => !up.User.IsProfilePrivate).ToList();
            }

            // Returnera alla deltagare om användaren är inloggad
            return usersInProject;
        }

        //Metod för att visa info
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
                .Include(p => p.User) // Inkludera skaparen av projektet
                .Include(p => p.UsersInProject)
                .ThenInclude(up => up.User)
                .AsQueryable();

            // Om användaren inte är inloggad, filtrera bort privata profiler
            if (!User.Identity.IsAuthenticated)
            {
                cvs = cvs.Where(cv => !cv.User.IsProfilePrivate); // Endast offentliga profiler
            }

            // Filtrera på förnamn om angivet
            if (!string.IsNullOrWhiteSpace(firstname))
            {
                firstname = firstname.ToLower(); // Gör input case-insensitive

                // Filtrera CV:n
                cvs = cvs.Where(cv => cv.User.Firstname.ToLower().StartsWith(firstname));

                // Filtrera projekt baserat på skaparen
                projects = projects.Where(p => p.User.Firstname.ToLower().StartsWith(firstname)
                    || p.UsersInProject.Any(up => up.User.Firstname.ToLower().StartsWith(firstname)));
            }

            // Skapa modellen för vyn
            var startPageViewModel = new StartPageViewModel
            {
                Cvs = await cvs.ToListAsync(),
                Projects = await projects.ToListAsync()
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
