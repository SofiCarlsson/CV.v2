using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CV_v2.Models;


namespace Projekt_CV_Site.Filters
{
    public class UnreadMessagesFilter : IActionFilter
    {
        private readonly UserContext _userContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnreadMessagesFilter(UserContext userContext, IHttpContextAccessor httpContextAccessor)
        {
            _userContext = userContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Task.Run(async () => await SetUnreadMessagesAsync()).Wait();
        }

        public async Task SetUnreadMessagesAsync()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return;

                var unreadMessages = await _userContext.Messages
                    .Where(m => m.TillUserId == userId && m.last == false)
                    .ToListAsync();

                if (unreadMessages.Any())
                {
                    _httpContextAccessor.HttpContext.Items["UnreadMessages"] = unreadMessages;
                }
                else
                {
                    _httpContextAccessor.HttpContext.Items["UnreadMessages"] = new List<Message>();
                }
            }
        }
    }
}
