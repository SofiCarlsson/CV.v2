using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
    public class CvWorkExperience
    {
        public int CVID { get; set; }
        [ForeignKey(nameof(CVID))]
        public virtual CV CV { get; set; }

        public int WorkExperienceID { get; set; }
        [ForeignKey(nameof(WorkExperienceID))]
        public virtual WorkExperience WorkExperience { get; set; }

    }

}
