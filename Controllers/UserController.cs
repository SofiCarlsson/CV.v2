using CV_v2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CV_v2.Controllers
{
    public class UserController : Controller
    {
        UserContext users;


        public UserController(UserContext service)
        {
            users = service;
        }

        [HttpGet]
        public IActionResult Index(string firstNameFilter)
        {
            IQueryable<User> userList = from user in users.Users
                                        where !user.IsProfilePrivate // Filtrera bort privata profiler
                                        select user;

            // Om firstNameFilter inte är tomt, filtrera användarna baserat på förnamn
            if (!string.IsNullOrEmpty(firstNameFilter))
            {
                userList = userList.Where(user => user.Firstname.ToLower().Contains(firstNameFilter.ToLower()));
            }

            // Skicka användarlistan till vyn
            ViewData["FirstNameFilter"] = firstNameFilter;
            return View("ShowUsers", userList.ToList());  // Returnera ShowUser-vyn istället för Index
        }




        [HttpPost]
        public IActionResult Add(User user)
        {
            if (ModelState.IsValid)
            {
                users.Add(user);
                users.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<SelectListItem> cvs = users.CVs.Select
                    (x => new SelectListItem { Text = x.CVId.ToString(), Value = x.CVId.ToString() }).ToList();
                ViewBag.options = cvs;
                return View(user);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ShowUsers()
        {
            // Ladda användare och deras tillhörande CV
            var usersList = await users.Users
                .Include(u => u.CV) // Inkludera relaterade CV-objekt
                .ToListAsync();

            return View(usersList);
        }
       
        [HttpGet]
        public ActionResult Test()
        {
            return View("test");
        }

        [HttpPost]
        public ActionResult Test(string user)
        {
            ViewBag.User = user;
            return View("test");
        }

        [HttpGet]
        public IActionResult Remove(string id)
        {
            User user = users.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult Remove(User user)
        {
            users.Remove(user);
            users.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}
