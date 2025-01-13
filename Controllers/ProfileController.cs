using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CV_v2.Models;

namespace CV_v2.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserContext _context;

        public ProfileController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return NotFound("Användare hittades inte.");
            }

            var model = new EditProfileViewModel
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                IsProfilePrivate = user.IsProfilePrivate // Lägg till detta
            };

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CVSite()
        {
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return NotFound();
            }

            var cv = await _context.CVs.FirstOrDefaultAsync(c => c.UserId == user.Id);

            var profileViewModel = new ProfileViewModel
            {
                User = user,
                CV = cv,
                Competences = cv?.Competences.ToList(),
                Educations = cv?.Educations.ToList(),
                WorkExperiences = cv?.WorkExperiences.ToList(),
                MyProjects = user.UserInProjects.ToList()
            };

            return View(profileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

                if (user == null)
                {
                    return NotFound("Användare hittades inte.");
                }

                user.Firstname = model.Firstname;
                user.Lastname = model.Lastname;
                user.Email = model.Email;
                user.IsProfilePrivate = model.IsProfilePrivate;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Profilen har uppdaterats.";
                return RedirectToAction("Edit");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile pictureFile)
        {
            if (pictureFile != null && pictureFile.Length > 0)
            {
                var username = User.Identity.Name;
                var user = await _context.Users.Include(u => u.CV).FirstOrDefaultAsync(u => u.UserName == username);

                if (user == null || user.CV == null)
                {
                    TempData["ErrorMessage"] = "Kunde inte hitta användaren eller deras CV.";
                    return RedirectToAction("Edit");
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", pictureFile.FileName);
                var newPicturePath = Path.Combine("images", pictureFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pictureFile.CopyToAsync(stream);
                }

                user.CV.PicturePath = newPicturePath;
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Profilbilden har uppdaterats.";
                return RedirectToAction("Edit");
            }

            TempData["ErrorMessage"] = "Ingen fil valdes.";
            return RedirectToAction("Edit");
        }
    }
}
