using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
	public class CV
	{
        [Key]
		public int CVId { get; set; }
        [Required(ErrorMessage = "Vänligen skriv en beskrivning.")]
        public string Description { get; set; }

        //Navigering till User.
        public virtual User User { get; set; }
        // Lägg till UserId för att koppla CV till en användare
        public string UserId { get; set; }  // Detta ska vara samma typ som User.Id
        [NotMapped]
        public IFormFile? PictureFile { get; set; }

        public string? PicturePath { get; set; }

        public virtual ICollection<CvWorkExperience>? WorkExperiences { get; set; }
        public virtual ICollection<CvEducation>? Educations { get; set; }
        public virtual ICollection<CvCompetences>? Competences { get; set; }



    }
}
