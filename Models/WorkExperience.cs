using System.ComponentModel.DataAnnotations;

namespace CV_v2.Models
{
    public class WorkExperience
    {
        public int WorkExperienceID { get; set; }
        [Required(ErrorMessage = "Vänligen ange jobbtitel.")]
        public string WorkTitle { get; set; }
      
        [Required(ErrorMessage = "Vänligen ange en beskrivning av arbetet.")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Vänligen ange startdatum för arbetet.")]
        public DateTime WorkStartDate { get; set; }
       
        [Required(ErrorMessage = "Vänligen ange slutdatum för arbetet.")]
        public DateTime WorkEndDate { get; set; }
        public virtual ICollection<CvWorkExperience> CVWorkExperiences { get; set; } = new List<CvWorkExperience>();

    }
}
