using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CV_v2.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> SendMessageAnon(string content, string anonym, string tillUserId)
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

        [Route("Message/SendMessage/{toUserName}")]
        [HttpGet]
        public async Task<IActionResult> SendMessage(string toUserName)
        {
            if (string.IsNullOrEmpty(toUserName))
            {
                return BadRequest("Användarnamn saknas.");
            }
            var toUser = await userContext.Users.FirstOrDefaultAsync(u => u.UserName == toUserName);

            var newMessage = new Message
            {
                TillUser = toUser,
                TillUserId = toUser.Id
            };
            ViewBag.RecipientUsername = toUserName;
            return View(newMessage);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message, string? Anonym, string toUserName)
        {

            var toUser = await userContext.Users.FirstOrDefaultAsync(u => u.UserName == toUserName);

            //Rensar modelstate annars blir den arg
            ModelState.Remove("TillUser");
            ModelState.Remove("TillUserId");

            message.TillUser = toUser;
            message.TillUserId = toUser.Id;

            if (!User.Identity.Name.IsNullOrEmpty())
            {
                //Hämtar id från den inloggade användaren
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

                message.FranUser = user;
                message.FranUserId = userId;
            }
            else
            {
                message.Anonym = Anonym;
            }

            userContext.Messages.Add(message);
            await userContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ShowMessages()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
  
            return View();
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






