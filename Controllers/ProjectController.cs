using CV_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace CV_v2.Controllers
{
    public class ProjectController : Controller
    {
        private readonly UserContext _context;

        public ProjectController(UserContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult ShowProjects()
        {
            var projects = _context.Projects.ToList();  // Hämtar alla projekt från databasen
            return View("~/Views/ProjectView/ShowProjects.cshtml", projects);  // Skickar projekten till vyn
        }

        [HttpPost]
        public IActionResult ShowProjects(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Add(project);  // Lägg till projektet i databasen
                _context.SaveChanges();  // Spara ändringarna i databasen
                return RedirectToAction("ShowProjects");  // Omdirigera tillbaka till visningen av alla projekt
            }

            // Om modellen inte är giltig, skicka tillbaka till vyn
            return View("~/Views/ProjectView/ShowProjects.cshtml", _context.Projects.ToList());
        }

        // GET: Projects
        [HttpGet]
        public IActionResult Index(string title)
        {
            Console.WriteLine("Index method reached");
            Console.WriteLine("Title filter: " + title);

            IQueryable<Project> projectList = from project in _context.Projects select project;

            if (!string.IsNullOrEmpty(title))
            {
                projectList = projectList.Where(p => p.Title.ToLower().Contains(title.ToLower()));
            }
            else
            {
                Console.WriteLine("No filtering applied");
            }

            Console.WriteLine($"Number of projects found: {projectList.Count()}");
            return View("~/Views/Home/Index.cshtml", projectList.ToList());
        }

        // GET: Project/Add
        [HttpGet]
        public IActionResult Add()
        {
            Project project = new Project();
            List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
            {
                Text = x.Firstname + " " + x.Lastname,
                Value = x.Id
            }).ToList();

            users.Insert(0, new SelectListItem { Text = "Select a user", Value = "" });
            ViewBag.Users = users;
            return View(project);
        }

        // POST: Project/Add
        [HttpPost]
        public IActionResult Add(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Add(project);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
                {
                    Text = x.Firstname + " " + x.Lastname,
                    Value = x.Id
                }).ToList();

                ViewBag.Users = users;
                return View(project);
            }
        }

        // GET: Project/Remove/5
        [HttpGet]
        public IActionResult Remove(int projectID)
        {
            Project project = _context.Projects.Find(projectID);
            return View(project);
        }

        // POST: Project/Remove
        [HttpPost]
        public IActionResult Remove(Project project)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // GET: Project/Details/5
        [HttpGet]
        public IActionResult Details(int projectID)
        {
            var project = _context.Projects
                .Include(p => p.User)
                .Include(p => p.UsersInProject)
                .ThenInclude(up => up.User)
                .FirstOrDefault(p => p.ProjectID == projectID);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
    }
}
