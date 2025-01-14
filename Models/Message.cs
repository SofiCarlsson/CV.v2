using CV_v2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CV_v2.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        [Required(ErrorMessage = "Vänligen fyll i meddelandefältet.")]
        public string Content { get; set; }
        public DateTime SentTime { get; set; } = DateTime.Now;
        public bool? last { get; set; }

        public string? FranUserId { get; set; }
        public string TillUserId { get; set;}   

        public string? Anonym { get; set; }

        [ForeignKey(nameof(FranUserId))]
        public virtual User? FranUser { get; set; }


        [ForeignKey(nameof(TillUserId))]
        public virtual User? TillUser { get; set; }

    }
}



