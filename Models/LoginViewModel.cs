using System.ComponentModel.DataAnnotations;

namespace CV_v2.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv ett användarnamn.")]
        [StringLength(255)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vänligen skriv lösenord.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}