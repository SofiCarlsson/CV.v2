using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CV_v2.Models;
using Microsoft.EntityFrameworkCore;

public class ProfilController : Controller
{
    private readonly UserContext _context;

    public ProfilController(UserContext context)
    {
        _context = context;
    }

    // Visa profil
    public async Task<IActionResult> Index()
    {
        var user = await _context.Users
            .Include(u => u.CV)  // Om du har en relation till CV
            .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

        if (user == null)
        {
            return NotFound();
        }

        var model = new EditProfileViewModel
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            CVId = user.CV?.CVId ?? 0
        };

        return View(model);
    }

    // Redigera profil
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var user = await _context.Users
            .Include(u => u.CV)  // Om du har en relation till CV
            .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

        if (user == null)
        {
            return NotFound();
        }

        var model = new EditProfileViewModel
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            CVId = user.CV?.CVId ?? 0
        };

        ViewBag.CVOptions = new List<string> {"CV1", "CV2", "CV3"};

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
            if (model.CVId != 0)
            {
                user.CV = await _context.CVs.FirstOrDefaultAsync(c => c.CVId == model.CVId);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit");
        }

        return View(model);
    }
}