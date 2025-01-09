using CV_v2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CV_v2.Controllers
{
    public class CVController : Controller
    {
        private readonly UserContext _context;

        public CVController(UserContext context)
        {
            _context = context;
        }

        ////Edit CV
        //[HttpGet]
        //public IActionResult Edit(string id)
        //{
        //   // var cv = _context.CVs.FirstOrDefault(c => c.UserId == id);  // Hitta CV baserat på UserId
        //    if (cv == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(cv);
        //}

        //Edit CV
        [HttpPost]
        public IActionResult Edit(CV updatedCV)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedCV);
            }

            var existingCV = _context.CVs.FirstOrDefault(c => c.CVId == updatedCV.CVId);
            if (existingCV != null)
            {
                existingCV.Competences = updatedCV.Competences;
                existingCV.Education = updatedCV.Education;
                existingCV.WorkExperience = updatedCV.WorkExperience;

                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(updatedCV);
        }

        //Create CV
        [HttpGet]
        public IActionResult Create()
        {
            // Hämtar den inloggade användarens ID
            string userId = User.Identity.Name;

            // Om användaren inte är inloggad, kan du omdirigera till inloggningssidan
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var newCV = new CV
            {
                //UserId = userId // Kopplar CV till rätt användare
            };
            return View(newCV);
        }

        //Create CV
        [HttpPost]
        public IActionResult Create(CV newCV)
        {
            // Kontrollera om modellen är giltig
            //if (!User.Identity.)
            //{
            //    return View(newCV); // Om modellen inte är giltig, visa formuläret igen
            //}

            // Hämtar användarens ID igen om det inte finns i CV:t
            string userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Om användaren inte är inloggad, omdirigera till inloggningssidan
            }

           /* newCV.UserId = userId;*/ // Säkerställ att rätt UserId är kopplat till CV:t

            // Lägg till det nya CV:t i databasen
            _context.CVs.Add(newCV);
            _context.SaveChanges(); // Spara ändringarna i databasen

            // Omdirigera till Edit-vyn för att visa det nyss skapade CV:t
            return RedirectToAction("Edit", "CV", new { id = userId });

        }


    }
}
