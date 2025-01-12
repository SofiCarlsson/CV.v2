namespace CV_v2.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public CV CV { get; set; }
        public List<CvCompetences> Competences { get; set; }
        public List<CvEducation> Educations { get; set; }
        public List<CvWorkExperience> WorkExperiences { get; set; }
        public List<UserInProject> MyProjects { get; set; }
    }
}
