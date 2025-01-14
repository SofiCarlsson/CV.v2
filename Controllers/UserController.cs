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
    }
}
