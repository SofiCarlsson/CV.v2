using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
    public class Competences
    {
        public int CompetencesID { get; set; }
        [Required(ErrorMessage = "Vänligen ange ett kompetensnamn.")]
        public string CompetenceName { get; set; }

        public virtual ICollection<CvCompetences> CvCompetences { get; set; } = new List<CvCompetences>();


    }
}
