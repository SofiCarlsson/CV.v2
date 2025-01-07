using CV_v2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Index(string id)
        {
            Console.WriteLine("Index method reached");
            Console.WriteLine("Id parameter: " + id);

            IQueryable<User> userList = from user in users.Users select user;

            if (!string.IsNullOrEmpty(id))
            {
                userList = userList.Where(user => user.Firstname.ToLower().Contains(id.ToLower()));
            }

            else
            {
                Console.WriteLine("No filtering applied");
            }

            Console.WriteLine($"Number of users found: {userList.Count()}");
            return View("~/Views/Home/Index.cshtml", userList.ToList());
        }

        [HttpGet]
		public IActionResult Add()
		{
			User user = new User();
            List<SelectListItem> cvs = users.CVs.Select(x => new SelectListItem
            { Text = x.CVId.ToString(), Value = x.CVId.ToString() }).ToList();
            cvs.Insert(0, new SelectListItem { Text = "Inget CV", Value = "" });
            ViewBag.options = cvs;
            return View(user);
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
        public IActionResult Remove(int id)
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
