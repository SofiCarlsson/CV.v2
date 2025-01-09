using System.ComponentModel.DataAnnotations;

namespace CV_v2.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv ett användarnamn.")]
        [StringLength(255)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vänligen skriv lösenord.")]
        [DataType(DataType.Password)]
        [Compare("BekraftaLosenord")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vänlingen bekräfta lösenordet")]
        [DataType(DataType.Password)]
        [Display(Name = "Bekrafta losenordet")]

        public string BekraftaLosenord { get; set; }

        [Required(ErrorMessage = "Vänligen skriv ett förnamn.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Vänligen skriv ett efternamn.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Vänligen skriv en adress.")]
        public string Address { get; set; }

    }
}
