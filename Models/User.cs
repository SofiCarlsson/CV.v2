using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
	public class User
	{
		[Required]
		public int UserId { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
		public int? CVID { get; set; }

		public virtual ICollection<Project> CreatedProjects { get; set; } = new List<Project>();

		//Fk CV
		[ForeignKey(nameof(CVID))]
		public virtual CV? CV { get; set; }

		public virtual ICollection<UserInProject> Projects { get; set; } = new List<UserInProject>();

	}
}
