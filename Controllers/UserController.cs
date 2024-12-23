using CV_v2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CV_v2.Controllers
{
	public class UserController : Controller
	{
		UserService users;

		public UserController(UserService service)
		{
			users = service;
		}

		[HttpGet]
		public IActionResult Add()
		{
			User user = new User();
			return View(user);
		}

		[HttpPost]
		public IActionResult Add(User user)
		{
			users.Add(user);
			return RedirectToAction("Index", "Home");
		}


		//Remove funkar inte just nu, vet inte varför.
		[HttpGet]
		public IActionResult Remove(int UserId)
		{
			for (int i = 0; i < users.Count; i++)
			{
				if (users.ElementAt(i).UserId.ToString().Equals(UserId.ToString()))
				{
					users.RemoveAt(i);
					break;
				}
			}
			return RedirectToAction("Index", "Home");
		}


	}
}
