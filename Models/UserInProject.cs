using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
    [PrimaryKey(nameof(UserId), nameof(ProjectId))]
    public class UserInProject
	{
		public string UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public virtual User User { get; set; }
		public int ProjectId { get; set; }
		[ForeignKey(nameof(ProjectId))]

		public virtual Project Project { get; set; }
	}
}


