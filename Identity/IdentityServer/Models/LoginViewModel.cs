using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Login")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
