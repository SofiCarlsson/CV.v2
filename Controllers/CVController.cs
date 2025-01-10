using CV_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                existingCV.Educations = updatedCV.Educations;
                existingCV.WorkExperiences = updatedCV.WorkExperiences;

                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(updatedCV);
        }

        // GET: Create CV
        [HttpGet]
        public IActionResult Create(string id)
        {
            var newCV = new CV
            {
                UserId = id // Kopplar CV till rätt användare
            };
            // Förbered alternativ för dropdown-menyer
            ViewBag.CompetenceOptions = _context.Competences
                .Select(c => new SelectListItem
                {
                    Value = c.CompetencesID.ToString(),
                    Text = c.CompetenceName
                })
                .ToList();

            ViewBag.EducationOptions = _context.Educations
                .Select(e => new SelectListItem
                {
                    Value = e.EducationID.ToString(),
                    Text = e.Degree + " - " + e.SchoolName
                })
                .ToList();

            ViewBag.WorkExperienceOptions = _context.WorkExperiences
                .Select(w => new SelectListItem
                {
                    Value = w.WorkExperienceID.ToString(),
                    Text = w.WorkTitle
                })
                .ToList();

            return View(newCV);
        }

        // POST: Create CV
        [HttpPost]
        public IActionResult Create(CV newCV, List<int> selectedCompetences, List<int> selectedEducations, List<int> selectedWorkExperiences)
        {
            if (!ModelState.IsValid)
            {
                // Om valideringen misslyckas, ladda om dropdown-alternativen
                ViewBag.CompetenceOptions = _context.Competences
                    .Select(c => new SelectListItem
                    {
                        Value = c.CompetencesID.ToString(),
                        Text = c.CompetenceName
                    })
                    .ToList();

                ViewBag.EducationOptions = _context.Educations
                    .Select(e => new SelectListItem
                    {
                        Value = e.EducationID.ToString(),
                        Text = e.Degree + " - " + e.SchoolName
                    })
                    .ToList();

                ViewBag.WorkExperienceOptions = _context.WorkExperiences
                    .Select(w => new SelectListItem
                    {
                        Value = w.WorkExperienceID.ToString(),
                        Text = w.WorkTitle
                    })
                    .ToList();

                return View(newCV); // Skicka tillbaka formuläret om valideringen misslyckas
            }

            _context.CVs.Add(newCV);
            _context.SaveChanges();

            // Koppla valda kompetenser till CV:t
            foreach (var competenceId in selectedCompetences)
            {
                var cvCompetence = new CvCompetences
                {
                    CVID = newCV.CVId,
                    CompetencesID = competenceId
                };
                _context.CvCompetences.Add(cvCompetence);
            }

            // Koppla valda utbildningar till CV:t
            foreach (var educationId in selectedEducations)
            {
                var cvEducation = new CvEducation
                {
                    CVID = newCV.CVId,
                    EducationID = educationId
                };
                _context.CvEducations.Add(cvEducation);
            }

            // Koppla valda arbetserfarenheter till CV:t
            foreach (var workExperienceId in selectedWorkExperiences)
            {
                var cvWorkExperience = new CvWorkExperience
                {
                    CVID = newCV.CVId,
                    WorkExperienceID = workExperienceId
                };
                _context.CvWorkExperiences.Add(cvWorkExperience);
            }

            // Spara ändringar i databasen
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AddEducation()
        {
            return View(new Education()); // Skicka en tom Education-modell till vyn
        }


        [HttpGet]
        public IActionResult AddWorkExperience()
        {
            return View(new WorkExperience()); // Skicka en instans av WorkExperience
        }


        [HttpGet]
        public IActionResult AddCompetence(int? id)
        {
            if (id == null) // Skapa ny kompetens
            {
                return View(new Competences());
            }

            var competence = _context.Competences.Find(id);
            if (competence == null)
            {
                return NotFound();
            }

            return View(competence);
        }

        [HttpPost]
        public IActionResult AddCompetence(Competences competence)
        {

            if (!ModelState.IsValid)
            {
                // Logga alla fel i ModelState
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
                return View(competence);
            }

            Console.WriteLine("ModelState is valid. Attempting to save...");
            _context.Competences.Add(competence);
            _context.SaveChanges();
            return RedirectToAction("Create", "CV");
        }
    }
}
