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
            var projects = _context.Projects.ToList();
            return View(projects);
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

            return View();
        }

        // POST: Handle adding a new project
        [HttpPost]
        public IActionResult Add(Project project)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(project.CreatedBy))
                {
                    ModelState.AddModelError("CreatedBy", "Välj en användare för att skapa projektet.");
                    List<SelectListItem> Users = _context.Users.Select(x => new SelectListItem
                    {
                        Text = x.Firstname + " " + x.Lastname,
                        Value = x.Id
                    }).ToList();
                    ViewBag.Users = Users;
                    return View(project);
                }

                var user = _context.Users.Find(project.CreatedBy);
                if (user != null)
                {
                    project.User = user;
                }

                _context.Projects.Add(project);
                _context.SaveChanges();

                _context.UserInProjects.Add(new UserInProject
                {
                    UserId = project.CreatedBy,
                    ProjectId = project.ProjectID
                });

                _context.SaveChanges();
                return RedirectToAction("ShowProjects");
            }

            List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
            {
                Text = x.Firstname + " " + x.Lastname,
                Value = x.Id
            }).ToList();
            ViewBag.Users = users;
            return View(project);
        }

        // GET: Show form to edit an existing project
        [HttpGet]
        public IActionResult EditProjects(int projectID)
        {
            var project = _context.Projects.Find(projectID);
            if (project == null)
            {
                return NotFound();
            }

            List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
            {
                Text = x.Firstname + " " + x.Lastname,
                Value = x.Id
            }).ToList();

            ViewBag.Users = users;
            return View(project);
        }

        // POST: Handle editing an existing project
        [HttpPost]
        public IActionResult EditProjects(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Update(project);
                _context.SaveChanges();
                return RedirectToAction("ShowProjects");
            }

            List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
            {
                Text = x.Firstname + " " + x.Lastname,
                Value = x.Id
            }).ToList();

            ViewBag.Users = users;
            return View(project);
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
                return NotFound();
            }

            return View(project);
        }

        // GET: Confirm project removal
        [HttpGet]
        public IActionResult Remove(int projectID)
        {
            var project = _context.Projects.Find(projectID);
            return View(project);
        }

        // POST: Handle project removal
        [HttpPost]
        public IActionResult Remove(Project project)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
            return RedirectToAction("ShowProjects");
        }
    }
}
