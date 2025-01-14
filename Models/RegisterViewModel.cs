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
        [Compare("BekraftaLosenord", ErrorMessage = "Lösenorden matchar inte.")]
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

        [Required(ErrorMessage = "Vänligen skriv en e-postadress.")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress.")]
        public string Email { get; set; }
        public bool IsProfilePrivate { get; set; }

    }
}
