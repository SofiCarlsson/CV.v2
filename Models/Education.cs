using System.ComponentModel.DataAnnotations;

namespace CV_v2.Models
{
    public class Education
    {
        public int EducationID { get; set; }
        [Required(ErrorMessage = "Vänligen ange skolans namn.")]
        public string SchoolName { get; set; }
        [Required(ErrorMessage = "Vänligen ange typ av utbildning.")]
        public string Degree { get; set; }
        [Required(ErrorMessage = "Vänligen ange startdatum.")]
        public DateTime EducationStartDate { get; set; }
        [Required(ErrorMessage = "Vänligen ange slutdatum.")]
        public DateTime EducationEndDate { get; set; }
        public virtual ICollection<CvEducation> CVEducations { get; set; } = new List<CvEducation>();


    }
}
