using CV_v2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CV_v2.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private UserService users;

		public HomeController(ILogger<HomeController> logger, UserService service)
		{
			_logger = logger;
			users = service;
		}

		public IActionResult Index()
		{
			return View(users);
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