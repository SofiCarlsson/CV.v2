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


        //// Metod för att skicka ett meddelande anonymt meddelande
        //[HttpPost]
        //public async Task<IActionResult> SendMessageAnon(string content, string anonym, string tillUserId)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            Message message = new Message
        //            {
        //                Content = content,
        //                SentTime = DateTime.Now,
        //                last = false,
        //                Anonym = anonym,
        //                TillUserId = tillUserId

        //            };

        //            userContext.Messages.Add(message);
        //            await userContext.SaveChangesAsync();
        //            return RedirectToAction("AllMessages");
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, $"Ett fel inträffade: {ex.Message}");
        //        }
        //    }

            
        //    return View("Error");
        //}

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

            message.last = false;
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

            var messages = await userContext.Messages
                .Include(m => m.FranUser)
                .Include(m => m.TillUser)
                .Where(m => m.TillUserId == userId || m.FranUserId == userId)
                .ToListAsync();

            var unreadMessages = messages.Where(m => m.last == false).ToList();
            var readMessages = messages.Where(m => m.last == true).ToList();

            ViewData["UnreadMessages"] = unreadMessages;
            ViewData["ReadMessages"] = readMessages;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeReadStatus(int messageId)
        {
            var message = await userContext.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId);
            if (message == null)
            {
                return NotFound();
            }

            message.last = !message.last;
            userContext.Update(message);
            await userContext.SaveChangesAsync();

            return RedirectToAction(nameof(ShowMessages));
        }

    }
}






