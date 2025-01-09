using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CV_v2.Models
{
    public class NewMessage
    {
        public SelectList? Users { get; set; }
        public string Message { get; set; }
        public string? SelectedUserId { get; set; }
        public List<Message> Messages { get; set; }
        public SelectList OlastaMessages { get; set; }
        public string? Anonym { get; set; }
        public string TillUserId { get; set; }

    }
}

