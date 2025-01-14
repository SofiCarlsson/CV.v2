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
        public string Address { get; set; } 

        public bool IsProfilePrivate { get; set; }
        public string Email { get; set; }

        public int? CVID { get; set; }

        [ForeignKey(nameof(CVID))]
        public virtual CV? CV { get; set; }

        public virtual ICollection<Project> CreatedProjects { get; set; } = new List<Project>();

        public virtual ICollection<UserInProject> UserInProjects { get; set; } = new List<UserInProject>();
    }
}

