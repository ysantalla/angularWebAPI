using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class SignUpViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}