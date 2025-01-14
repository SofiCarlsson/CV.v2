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


        //Metod f�r att h�lla koll p� om Anv�ndaren �r privat.
        private IEnumerable<UserInProject> FilterPrivateProfiles(IEnumerable<UserInProject> usersInProject, bool isAuthenticated)
        {
            if (!isAuthenticated)
            {
                // Filtrera bort deltagare med privata profiler om anv�ndaren inte �r inloggad
                return usersInProject.Where(up => !up.User.IsProfilePrivate).ToList();
            }

            // Returnera alla deltagare om anv�ndaren �r inloggad
            return usersInProject;
        }

        //Metod f�r att visa info
        [HttpGet]
        public async Task<IActionResult> Index(string firstname)
        {
            // H�mta alla CV:n och projekt
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

            // Om anv�ndaren inte �r inloggad, filtrera bort privata profiler
            if (!User.Identity.IsAuthenticated)
            {
                cvs = cvs.Where(cv => !cv.User.IsProfilePrivate); // Endast offentliga profiler
            }

            // Filtrera p� f�rnamn om angivet
            if (!string.IsNullOrWhiteSpace(firstname))
            {
                firstname = firstname.ToLower(); // G�r input case-insensitive

                // Filtrera CV:n
                cvs = cvs.Where(cv => cv.User.Firstname.ToLower().StartsWith(firstname));

                // Filtrera projekt baserat p� skaparen
                projects = projects.Where(p => p.User.Firstname.ToLower().StartsWith(firstname)
                    || p.UsersInProject.Any(up => up.User.Firstname.ToLower().StartsWith(firstname)));
            }

            // Skapa modellen f�r vyn
            var startPageViewModel = new StartPageViewModel
            {
                Cvs = await cvs.ToListAsync(),
                Projects = await projects.ToListAsync()
            };

            // Skicka tillbaka s�kparametern f�r att �terfylla s�kf�ltet
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
