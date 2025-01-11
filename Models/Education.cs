namespace CV_v2.Models
{
    public class Education
    {
        public int EducationID { get; set; }
        public string SchoolName { get; set; }
        public string Degree { get; set; }
        public DateTime EducationStartDate { get; set; }
        public DateTime EducationEndDate { get; set; }
        public virtual ICollection<CvEducation> CVEducations { get; set; } = new List<CvEducation>();


    }
}
