using CV_v2.Models;
using Microsoft.AspNetCore.Authorization;
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
            var projects = _context.Projects
                .Include(p => p.UsersInProject)
                .ThenInclude(up => up.User)
                .Include(p => p.User) // Inkludera skaparen av projektet
                .AsQueryable();

            // Om användaren inte är inloggad, filtrera bort projekt från privata profiler
            if (!User.Identity.IsAuthenticated)
            {
                projects = projects.Where(p => p.User != null && !p.User.IsProfilePrivate);
            }

            return View(projects.ToList());
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

            users.Insert(0, new SelectListItem { Text = "Välj en användare", Value = "" });
            ViewBag.Users = users;

            return View();
        }

        // POST: Handle adding a new project
        [HttpPost]
        public IActionResult Add(Project project)
        {
            ModelState.Remove("User");
            var user = _context.Users.Find(project.CreatedBy);
            project.User = user;

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
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Valideringsfel: " + error.ErrorMessage);
                }
            }

            List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
            {
                Text = x.Firstname + " " + x.Lastname,
                Value = x.Id
            }).ToList();
            ViewBag.Users = users;
            return View(project);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> JoinProject(int projectId)
        {
            // Hämta den inloggade användarens ID
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Du måste vara inloggad för att gå med i ett projekt.";
                return RedirectToAction("Index", "Home");
            }

            // Kontrollera om projektet existerar
            var project = await _context.Projects
                .Include(p => p.UsersInProject)
                .FirstOrDefaultAsync(p => p.ProjectID == projectId);

            if (project == null)
            {
                TempData["Error"] = "Projektet kunde inte hittas.";
                return RedirectToAction("Index", "Home");
            }

            // Kontrollera om användaren redan är med i projektet
            if (project.UsersInProject.Any(up => up.UserId == userId))
            {
                TempData["Error"] = "Du är redan med i det här projektet.";
                return RedirectToAction("Index", "Home");
            }

            // Lägg till användaren till projektet
            var userInProject = new UserInProject
            {
                ProjectId = projectId,
                UserId = userId
            };

            _context.UserInProjects.Add(userInProject);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Du har gått med i projektet!";
            return RedirectToAction("Index", "Home");
        }


        // GET: Show form to edit an existing project
        [HttpGet]
        public IActionResult EditProjects(int projectID)
        {
            var project = _context.Projects
                .Include(p => p.User)
                .FirstOrDefault(p => p.ProjectID == projectID);

            if (project == null)
            {
                return NotFound();
            }

            // Kontrollera om den inloggade användaren är skaparen
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (project.CreatedBy != userId)
            {
                ViewBag.ErrorMessage = "Du har inte behörighet att redigera detta projekt.";
                return View("ShowProjects", _context.Projects.ToList());
            }

            List<SelectListItem> users = _context.Users.Select(x => new SelectListItem
            {
                Text = x.Firstname + " " + x.Lastname,
                Value = x.Id
            }).ToList();

            ViewBag.Users = users;
            return View(project);
        }

        [HttpPost]
        public IActionResult EditProjects(Project project)
        {
            // Kontrollera om projektet existerar
            var existingProject = _context.Projects.Find(project.ProjectID);
            if (existingProject == null)
            {
                return NotFound();
            }

            // Kontrollera om den inloggade användaren är skaparen
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (existingProject.CreatedBy != userId)
            {
                ViewBag.ErrorMessage = "Du har inte behörighet att redigera detta projekt.";
                return View("ShowProjects", _context.Projects.ToList());
            }

            // Uppdatera endast titel och beskrivning
            existingProject.Title = project.Title;
            existingProject.Description = project.Description;

            ModelState.Remove("User");
            ModelState.Remove("CreatedBy");
            var user = _context.Users.Find(project.CreatedBy);
            existingProject.User = user;
            existingProject.CreatedBy = userId;



            if (ModelState.IsValid)
            {
                _context.Projects.Update(existingProject);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Projektet har uppdaterats.";
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
