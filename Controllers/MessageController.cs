using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CV_v2.Models;

namespace Projekt_CV_Site.Controllers
{
    public class MessageController : Controller
    {
        private List<Message> messages = new List<Message>();

        private readonly UserContext userContext;
        private readonly UserManager<User> userManager;

        public MessageController(UserContext userContext, UserManager<User> userManager)
        {
            this.userContext = userContext;
            this.userManager = userManager;
        }

        //Denna metod räknar alla meddelanden med Last=false
        [HttpGet]
        public async Task<IActionResult> RaknaOlasta()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var currentUserID = currentUser?.Id ?? string.Empty; 

            var count = await userContext.Messages
                                       .CountAsync(m => m.TillUserId == currentUserID && m.last == false);
            return Json(count);
        }

        //Denna metod öppnar AnonymMessage 
        [HttpGet]
        public IActionResult AnonymMessage(string tillUserId)
        {
            var model = new NewMessage
            {
                TillUserId = tillUserId
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AllMessages()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var currentUserID = currentUser?.Id ?? string.Empty; 

            // Hämta en lista över Users
            var registeredUsers = await userContext.Users
                .Select(u => new SelectListItem { Value = u.Id, Text = u.UserName })  
                .ToListAsync();

            var anonymousNames = await userContext.Messages
                .Where(m => !string.IsNullOrWhiteSpace(m.Anonym))
                .Select(m => m.Anonym)
                .Distinct()
                .ToListAsync();

            // Skapa en lista över både registrerade användare och anonyma.
            var combinedList = new List<SelectListItem>();
    
            combinedList.AddRange(registeredUsers);

            foreach (var anonymName in anonymousNames)
            {
                // Kontrollera så att namnet inte redan finns bland de registrerade användarnamnen
                if (!combinedList.Any(u => u.Text.Equals(anonymName, StringComparison.OrdinalIgnoreCase)))
                {
                    combinedList.Add(new SelectListItem { Value = "Anonym-" + anonymName, Text = anonymName });
                }
            }

            var viewModel = new NewMessage
            {
                Users = new SelectList(combinedList, "Value", "Text"),
            };

            return View(viewModel);
        }

        // Metod för att skicka ett meddelande anonymt meddelande
        [HttpPost]
        public async Task<IActionResult> SkickaAnonymMessage(string content, string anonym, string tillUserId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Message message = new Message
                    {
                        Content = content,
                        SentTime = DateTime.Now,
                        last = false,
                        Anonym = anonym,
                        TillUserId = tillUserId

                    };

                    userContext.Messages.Add(message);
                    await userContext.SaveChangesAsync();
                    return RedirectToAction("AllMessages");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Ett fel inträffade: {ex.Message}");
                }
            }

            
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> VisaOlastaMessages(string selectedUserId)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var currentUserID = currentUser?.Id ?? string.Empty; 

            var viewModel = new NewMessage();

            var olastaMessages = await userContext.Messages
                .Where(m => m.last == false && m.TillUserId == currentUserID)
                .Include(m => m.FranUser)
                .ToListAsync();

            var messageSenders = olastaMessages
            .Select(m => new SelectListItem
            {
                // Kontrollera om FranUserId är null, och använd istället m.anonym, annars FranUserId
                Value = m.FranUserId ?? m.Anonym, 
                Text = m.FranUser != null ? m.FranUser.UserName : m.Anonym 
            })
             .Distinct()
             .ToList();

            viewModel.OlastaMessages = new SelectList(messageSenders, "Value", "Text");

            return View("AllMessages", viewModel);
        }

        //Metod för att andra meddelande till läst.
        [HttpPost]
        public async Task<IActionResult> MarkeraSomLast(int messageId)
        {
            var message = await userContext.Messages.FindAsync(messageId);
            if (message != null)
            {
                message.last = true;
                userContext.Update(message);
                await userContext.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult NyttMessage()
        {
            var users = userContext.Users.ToList();
            var viewModel = new NewMessage
            {
                Users = new SelectList(users, "Id", "UserName"),  
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> NewMessages()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var currentUser = await userManager.GetUserAsync(User);
            var currentUserID = currentUser?.Id ?? string.Empty;  

            var viewModel = new NewMessage
            {
                Users = new SelectList(await userContext.Users.ToListAsync(), "Id", "UserName")  
            };

            var olastaMessages = await userContext.Messages
                .Where(m => m.last == false && m.TillUserId == currentUserID)
                .Include(m => m.FranUser)
                .ToListAsync();

            if (olastaMessages.Any())
            {
                ViewBag.UserMessagesOlasta = olastaMessages;
            }
            else
            {
                ViewBag.UserMessagesOlasta = new List<Message>();
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> VisaMessages(string selectedUserId)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var currentUserID = currentUser?.Id ?? string.Empty;  

            
            var viewModel = new NewMessage();

           
            var users = await userContext.Users.ToListAsync();
            viewModel.Users = new SelectList(users, "Id", "UserName");  

            if (!string.IsNullOrEmpty(selectedUserId) && !string.IsNullOrEmpty(currentUserID))
            {
                var userMessages = await userContext.Messages
                    .Include(m => m.FranUser)
                    .Where(m => (m.TillUserId == selectedUserId && m.FranUserId == currentUserID)
                             || (m.FranUserId == selectedUserId && m.TillUserId == currentUserID))
                    .OrderBy(m => m.SentTime)
                    .ToListAsync();

                ViewBag.UserMessages = userMessages;
            }
            return View("AllMessages", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SkickaNyttMessage(string innehall, string tillAnvandareId)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await userManager.GetUserAsync(User); // Hämta den inloggade användaren 
                try
                {
                    Message message = new Message
                    {
                        Content = innehall,
                        SentTime = DateTime.Now,
                        last = false,
                        FranUserId = currentUser?.Id,  
                        TillUserId = tillAnvandareId
                    };

                    userContext.Messages.Add(message);
                    await userContext.SaveChangesAsync();
                    return RedirectToAction("AllMessages");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Ett fel inträffade: {ex.Message}");
                }
            }

            return View("Error"); 
        }
    }
}






