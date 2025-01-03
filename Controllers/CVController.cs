using Microsoft.AspNetCore.Mvc;
using CV_v2.Models;

namespace CV_v2.Controllers
{
    public class CVController : Controller
    {
        private readonly UserContext _context;

        public CVController(UserContext context)
        {
            _context = context;
        }

        // GET: CV/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CV/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CV cv)
        {
            if (ModelState.IsValid)
            {
                // Här kan du sätta UserId om det inte redan är satt
                cv.UserId = 1; // Eller hämta UserId från den inloggade användaren

                _context.CVs.Add(cv);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home"); // Eller en annan vy du vill
            }

            return View(cv);
        }

        // GET: CV/Edit/5
        public IActionResult Edit(int id)
        {
            var cv = _context.CVs.FirstOrDefault(c => c.CVId == id);

            if (cv == null)
            {
                return NotFound();
            }

            return View(cv);
        }

        // POST: CV/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CV cv)
        {
            if (id != cv.CVId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.CVs.Update(cv);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home"); // Eller en annan vy du vill
            }

            return View(cv);
        }
    }
}
