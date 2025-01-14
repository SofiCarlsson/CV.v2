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
        public virtual User User { get; set; }
        public string UserId { get; set; }  
        [NotMapped]
        public IFormFile? PictureFile { get; set; }

        public string? PicturePath { get; set; }

        public virtual ICollection<CvWorkExperience>? WorkExperiences { get; set; }
        public virtual ICollection<CvEducation>? Educations { get; set; }
        public virtual ICollection<CvCompetences>? Competences { get; set; }



    }
}
