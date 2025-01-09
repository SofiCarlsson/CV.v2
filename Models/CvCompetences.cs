using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
    public class CvCompetences
    {
        public int CVID { get; set; }

        public int CompetencesID { get; set; }

        [ForeignKey(nameof(CVID))]
        public virtual CV CV { get; set; }

        [ForeignKey(nameof(CompetencesID))]
        public virtual Competences Competences { get; set; }
    }
}
