using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
    public class Project
    {
        public int ProjectID { get; set; }

        [Required(ErrorMessage = "Vänligen ange en titel för projektet.")]
        [StringLength(100, ErrorMessage = "Titeln får max vara 100 tecken lång.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vänligen ange en beskrivning av projektet.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vänligen välj en skapare för projektet.")]
        public string CreatedBy { get; set; }
            
        [ForeignKey(nameof(CreatedBy))]
        public virtual User User { get; set; }
        public virtual ICollection<UserInProject> UsersInProject { get; set; } = new List<UserInProject>();
    }
}
