using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
	public class User
	{
		public int UserId { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public int? CVID { get; set; }

		public virtual ICollection<Project> CreatedProjects { get; set; } = new List<Project>();

		//Fk CV
		[ForeignKey(nameof(CVID))]
		public virtual CV CV { get; set; }

		public virtual ICollection<UserInProject> Projects { get; set; } = new List<UserInProject>();

	}
}
