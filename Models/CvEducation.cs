using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
    public class CvEducation
    {
        public int CVID { get; set; }
        [ForeignKey(nameof(CVID))]
        public virtual CV CV { get; set; }

        public int EducationID { get; set; }
        [ForeignKey(nameof(EducationID))]
        public virtual Education Education { get; set; }

    }
}
