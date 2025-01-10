namespace CV_v2.Models
{
    public class WorkExperience
    {
        public int WorkExperienceID { get; set; }
        public string WorkTitle { get; set; }
        public string Description { get; set; }
        public DateTime WorkStartDate { get; set; }
        public DateTime WorkEndDate { get; set; }
        public virtual ICollection<CvWorkExperience> CVWorkExperiences { get; set; }

    }
}
