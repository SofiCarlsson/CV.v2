using System.ComponentModel.DataAnnotations;

namespace CV_v2.Models
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Vänligen ange ett förnamn.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Vänligen ange ett efternamn.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Vänligen ange en e-postadress.")]
        [EmailAddress(ErrorMessage = "Vänligen ange en giltig e-postadress.")]
        public string Email { get; set; }

        public bool IsProfilePrivate { get; set; }

        [Required(ErrorMessage = "Vänligen ange en adress.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vänligen skriv ditt lösenord.")]
        [DataType(DataType.Password)]
        [Display(Name = "Gammalt lösenord")]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "Vänligen skriv ett lösenord.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nytt lösenord")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Vänligen skriv ett lösenord.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte.")]
        [Display(Name = "Bekräfta nytt lösenord")]
        public string ConfirmNewPassword { get; set; }
    }
}
