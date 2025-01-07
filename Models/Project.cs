using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
	public class Project
	{
		public int ProjectID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		// 1:M FK user  CreatedBy (referens till User)
		public string CreatedBy { get; set; }
		
		[ForeignKey(nameof(CreatedBy))]
		public virtual User User { get; set; }

		public virtual ICollection<UserInProject> UsersInProject { get; set; } = new List<UserInProject>();

	}
}
