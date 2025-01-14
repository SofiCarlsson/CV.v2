using CV_v2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace CV_v2.Controllers
{
    public class CVController : Controller
    {
        private readonly UserContext _context;

        public CVController(UserContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var cv = _context.CVs
                .Include(c => c.Competences)
                .ThenInclude(cc => cc.Competences)
                .Include(c => c.Educations)
                .ThenInclude(ce => ce.Education)
                .Include(c => c.WorkExperiences)
                .ThenInclude(cw => cw.WorkExperience)
                .FirstOrDefault(c => c.CVId == id);

            if (cv == null)
            {
                return NotFound();
            }

            ViewBag.CompetenceOptions = _context.Competences.Select(c => new SelectListItem
            {
                Value = c.CompetencesID.ToString(),
                Text = c.CompetenceName,
                Selected = cv.Competences.Any(cc => cc.CompetencesID == c.CompetencesID)
            }).ToList();

            ViewBag.EducationOptions = _context.Educations.Select(e => new SelectListItem
            {
                Value = e.EducationID.ToString(),
                Text = e.Degree + " - " + e.SchoolName,
                Selected = cv.Educations.Any(ce => ce.EducationID == e.EducationID)
            }).ToList();

            ViewBag.WorkExperienceOptions = _context.WorkExperiences.Select(w => new SelectListItem
            {
                Value = w.WorkExperienceID.ToString(),
                Text = w.WorkTitle,
                Selected = cv.WorkExperiences.Any(cw => cw.WorkExperienceID == w.WorkExperienceID)
            }).ToList();

            return View(cv);
        }

        [HttpPost]
        public IActionResult Edit(CV updatedCV, List<int> Competences, List<int> Educations, List<int> WorkExperiences)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CompetenceOptions = _context.Competences.Select(c => new SelectListItem
                {
                    Value = c.CompetencesID.ToString(),
                    Text = c.CompetenceName
                }).ToList();

                ViewBag.EducationOptions = _context.Educations.Select(e => new SelectListItem
                {
                    Value = e.EducationID.ToString(),
                    Text = e.Degree + " - " + e.SchoolName
                }).ToList();

                ViewBag.WorkExperienceOptions = _context.WorkExperiences.Select(w => new SelectListItem
                {
                    Value = w.WorkExperienceID.ToString(),
                    Text = w.WorkTitle
                }).ToList();

                return View(updatedCV);
            }

            var existingCV = _context.CVs
                .Include(c => c.Competences)
                .Include(c => c.Educations)
                .Include(c => c.WorkExperiences)
                .FirstOrDefault(c => c.CVId == updatedCV.CVId);

            if (existingCV != null)
            {
                existingCV.Description = updatedCV.Description;

                existingCV.Competences.Clear();
                foreach (var competenceId in Competences)
                {
                    var competence = _context.Competences.Find(competenceId);
                    if (competence != null)
                    {
                        existingCV.Competences.Add(new CvCompetences { CV = existingCV, Competences = competence });
                    }
                }

                existingCV.Educations.Clear();
                foreach (var educationId in Educations)
                {
                    var education = _context.Educations.Find(educationId);
                    if (education != null)
                    {
                        existingCV.Educations.Add(new CvEducation { CV = existingCV, Education = education });
                    }
                }

                existingCV.WorkExperiences.Clear();
                foreach (var workExperienceId in WorkExperiences)
                {
                    var workExperience = _context.WorkExperiences.Find(workExperienceId);
                    if (workExperience != null)
                    {
                        existingCV.WorkExperiences.Add(new CvWorkExperience { CV = existingCV, WorkExperience = workExperience });
                    }
                }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Dina ändringar har sparats!";
                return RedirectToAction("Edit", new { id = existingCV.CVId });
            }

            return View(updatedCV);
        }


        // GET: Create CV
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var newCV = new CV{};

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
        public async Task<IActionResult> Create(CV newCV, List<int> Competences, List<int> Educations, List<int> WorkExperiences)
        {
           
            //Hämtar id från den inloggade användaren
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            //Rensar modelstate annars blir den arg
            ModelState.Remove("User");
            ModelState.Remove("UserId");

            //Sätter UserID i CV till den inloggade användaren
            newCV.UserId = userId;
            newCV.User = user;

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Valideringsfel: " + error.ErrorMessage);
                }
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
            await _context.SaveChangesAsync();


            // Koppla valda kompetenser till CV:t
            foreach (var competence in Competences)
            {
                var cvCompetence = new CvCompetences
                {
                    CVID = newCV.CVId,
                    CompetencesID = competence
                };
                _context.CvCompetences.Add(cvCompetence);
            }

            // Koppla valda utbildningar till CV:t
            foreach (var educations in Educations)
            {
                var cvEducation = new CvEducation
                {
                    CVID = newCV.CVId,
                    EducationID = educations
                };
                _context.CvEducations.Add(cvEducation);
            }

            // Koppla valda arbetserfarenheter till CV:t
            foreach (var workExperiences in WorkExperiences)
            {
                var cvWorkExperience = new CvWorkExperience
                {
                    CVID = newCV.CVId,
                    WorkExperienceID = workExperiences
                };
                _context.CvWorkExperiences.Add(cvWorkExperience);
            }

            // Spara ändringar i databasen
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AddEducation()
        {
            return View(new Education()); // Skicka en tom Education-modell till vyn
        }

        [HttpPost]
        public IActionResult AddEducation(Education education)
        {

            if (!ModelState.IsValid)
            {
                return View(education);
            }
            _context.Educations.Add(education);
            _context.SaveChanges();
            return RedirectToAction("Create", "CV");
        }


        [HttpGet]
        public IActionResult AddWorkExperience()
        {
            return View(new WorkExperience()); // Skicka en instans av WorkExperience
        }

        [HttpPost]
        public IActionResult AddWorkExperience(WorkExperience workExperience)
        {

            if (!ModelState.IsValid)
            {
                return View(workExperience);
            }
            _context.WorkExperiences.Add(workExperience);
            _context.SaveChanges();
            return RedirectToAction("Create", "CV");
        }


        [HttpGet]
        public IActionResult AddCompetence()

        {
            return View(new Competences()); // Skicka en instans av Competences
        }

        [HttpPost]
        public IActionResult AddCompetence(Competences competence)
        {

            if (!ModelState.IsValid)
            {
                return View(competence);
            }
            _context.Competences.Add(competence);
            _context.SaveChanges();
            return RedirectToAction("Create", "CV");
        }
    }
}
