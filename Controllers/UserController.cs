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


        //Metod för att kunna filtrera användare efter bokstav (de privata profilerna visas inte)
        [HttpGet]
        public IActionResult Index(string firstNameFilter)
        {
            IQueryable<User> userList = from user in users.Users
                                        where !user.IsProfilePrivate 
                                        select user;
            
            if (!string.IsNullOrEmpty(firstNameFilter))
            {
                userList = userList.Where(user => user.Firstname.ToLower().Contains(firstNameFilter.ToLower()));
            }

            ViewData["FirstNameFilter"] = firstNameFilter;
            return View("ShowUsers", userList.ToList()); 
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

        //När man tycker på fliken Användare vissas alla användare utom de som är private.
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ShowUsers()
        {
      
            var usersList = await users.Users
                .Where(u => !u.IsProfilePrivate) 
                .Include(u => u.CV) 
                .ToListAsync();

            return View(usersList);
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
