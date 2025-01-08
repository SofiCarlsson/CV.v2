using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_v2.Models
{
	public class CV
	{
        [Key]
		public int CVId { get; set; }

        //Navigering till User.
        public virtual User User { get; set; }
        // Lägg till UserId för att koppla CV till en användare
        //public string UserId { get; set; }  // Detta ska vara samma typ som User.Id
        public string? CompetencesHej { get; set; }
		public string? Education { get; set; }
		public string? WorkExperience { get; set; }

        

    }
}
