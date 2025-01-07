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

        // GET: Edit CV
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cv = _context.CVs.FirstOrDefault(c => c.CVId == id);
            if (cv == null)
            {
                return NotFound();
            }
            return View(cv);
        }

        // POST: Edit CV
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

        // GET: Create CV
        [HttpGet]
        public IActionResult Create(int id)
        {
            var newCV = new CV
            {
                UserId = id // Kopplar CV till rätt användare
            };
            return View(newCV);
        }

        // POST: Create CV
        [HttpPost]
        public IActionResult Create(CV newCV)
        {
            if (!ModelState.IsValid)
            {
                return View(newCV);
            }

            _context.CVs.Add(newCV);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
