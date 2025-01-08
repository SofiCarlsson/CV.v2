using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
	public class User : IdentityUser
    {
        [Required(ErrorMessage = "Förnamn måste anges")]
        public string Firstname { get; set; }
     
		[Required(ErrorMessage = "Efternamn måste anges")]
		public string Lastname { get; set; }
        // Lägg till UserId för att koppla CV till en användare

        public int? CVID { get; set; }

        //Fk CV
        [ForeignKey(nameof(CVID))]
        public virtual CV? CV { get; set; }

        public virtual ICollection<Project> CreatedProjects { get; set; } = new List<Project>();

		public virtual ICollection<UserInProject> Projects { get; set; } = new List<UserInProject>();

	}
}
