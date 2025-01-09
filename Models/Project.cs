using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // FK till User (den som skapade projektet)
        public string CreatedBy { get; set; }

        // Relation till User (den som skapade projektet)
        [ForeignKey(nameof(CreatedBy))]
        public virtual User User { get; set; }

        // Många-till-många-relation mellan User och Project via UserInProject
        public virtual ICollection<UserInProject> UsersInProject { get; set; } = new List<UserInProject>();

        // Navigeringsegenskap för projekt som användaren är associerad med via UserInProject
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
