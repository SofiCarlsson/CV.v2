using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
	public class UserInProject
	{
		public int UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public virtual User User { get; set; }
		public int ProjectId { get; set; }
		[ForeignKey(nameof(ProjectId))]

		public virtual Project Project { get; set; }
	}
}
