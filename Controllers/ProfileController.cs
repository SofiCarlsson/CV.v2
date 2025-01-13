using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CV_v2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

public class ProfileController : Controller
{
    private readonly UserContext _context;

    public ProfileController(UserContext context)
    {
        _context = context;
    }

    // Visa profil
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var username = User.Identity.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        var userId = user.Id;
        var cv = await _context.CVs.FirstOrDefaultAsync(c => c.UserId == userId);

        var model = new EditProfileViewModel
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            Cv = cv
        };

        return View(model);
    }

    // Redigera profil
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var username = User.Identity.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        var userId = user.Id;
        var cv = await _context.CVs.FirstOrDefaultAsync(c => c.UserId == userId);

        var model = new EditProfileViewModel
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            Cv = cv
        };


        return View(model);
    }

    // Uppdatera profil
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users
                .Include(u => u.CV)  // Om du har en relation till CV
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Email = model.Email;

            // Om användaren har ett CV, uppdatera det
            if (model.Cv.CVId != 0)
            {
                user.CV = await _context.CVs.FirstOrDefaultAsync(c => c.CVId == model.Cv.CVId);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Profilen har uppdaterats";

            return RedirectToAction("Edit");
        }

        return View(model);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> CVSite()
    {
        var username = User.Identity.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        var userId = user.Id;
        var cv = await _context.CVs.FirstOrDefaultAsync(c => c.UserId == userId);

        var profileViewModel = new ProfileViewModel
        {
            User = user,
            CV = cv,
            Competences = cv.Competences.ToList(),
            Educations = cv.Educations.ToList(),
            WorkExperiences = cv.WorkExperiences.ToList(),
            MyProjects = user.UserInProjects.ToList()
        };

        return View(profileViewModel);
    }


    [HttpPost]
    public async Task<IActionResult> UploadImage(EditProfileViewModel model)
    {

        if (model.Cv != null && model.Cv.PictureFile.Length > 0)
        {
            //Hämta CV
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            var userId = user.Id;
            var cv = await _context.CVs.FirstOrDefaultAsync(c => c.UserId == userId);

            // Spara sökvägen och bilden
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", model.Cv.PictureFile.FileName);
            var newPicturePath = Path.Combine("images", model.Cv.PictureFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Spara filen till den angivna platsen
                await model.Cv.PictureFile.CopyToAsync(stream);
            }

            cv.PicturePath = newPicturePath;
            
            await _context.SaveChangesAsync();

            return RedirectToAction("CVSite");
        }

        return View(model);
    }

}