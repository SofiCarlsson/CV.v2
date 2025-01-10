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

        // GET: Show all projects
        [HttpGet]
        public IActionResult ShowProjects()
        {
            var projects = _context.Projects.ToList();  // Hämtar alla projekt från databasen
            return View(projects);  // Razor söker nu automatiskt i /Views/Project/ShowProjects.cshtml
        }

        // GET: Show form to add a new project
        [HttpGet]
        public IActionResult Add()
        {
            List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
            {
                Text = x.Firstname + " " + x.Lastname,
                Value = x.Id
            }).ToList();

            users.Insert(0, new SelectListItem { Text = "Select a user", Value = "" });
            ViewBag.Users = users;

            return View(); // Razor söker i /Views/Project/Add.cshtml
        }

        // POST: Handle adding a new project
        [HttpPost]
        public IActionResult Add(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Add(project);
                _context.SaveChanges();
                return RedirectToAction("ShowProjects");  // Omdirigera till ShowProjects efter att projektet är skapat
            }

            // Om modellen inte är giltig, ladda om formuläret med användare
            List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
            {
                Text = x.Firstname + " " + x.Lastname,
                Value = x.Id
            }).ToList();

            ViewBag.Users = users;
            return View(project); // Razor söker i /Views/Project/Add.cshtml
        }

        // GET: Show form to edit an existing project
        [HttpGet]
        public IActionResult EditProjects(int projectID)
        {
            var project = _context.Projects.Find(projectID);
            if (project == null)
            {
                return NotFound(); // Returnera 404 om projektet inte hittas
            }

            List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
            {
                Text = x.Firstname + " " + x.Lastname,
                Value = x.Id
            }).ToList();

            ViewBag.Users = users;
            return View(project); // Razor söker i /Views/Project/EditProjects.cshtml
        }

        // POST: Handle editing an existing project
        [HttpPost]
        public IActionResult EditProjects(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Update(project);
                _context.SaveChanges();
                return RedirectToAction("ShowProjects");  // Omdirigera tillbaka till ShowProjects efter uppdatering
            }

            // Om modellen inte är giltig, ladda om formuläret med användare
            List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
            {
                Text = x.Firstname + " " + x.Lastname,
                Value = x.Id
            }).ToList();

            ViewBag.Users = users;
            return View(project); // Razor söker i /Views/Project/EditProjects.cshtml
        }

        // GET: Show details for a specific project
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
                return NotFound(); // Returnera 404 om projektet inte hittas
            }

            return View(project); // Razor söker i /Views/Project/Details.cshtml
        }

        // GET: Confirm project removal
        [HttpGet]
        public IActionResult Remove(int projectID)
        {
            var project = _context.Projects.Find(projectID);
            return View(project); // Razor söker i /Views/Project/Remove.cshtml
        }

        // POST: Handle project removal
        [HttpPost]
        public IActionResult Remove(Project project)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
            return RedirectToAction("ShowProjects");  // Omdirigera tillbaka till ShowProjects efter borttagning
        }
    }
}
