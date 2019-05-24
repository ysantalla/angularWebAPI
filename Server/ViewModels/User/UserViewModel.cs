using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class UserViewModel
    {
        public long Id { get; set; } = 0;
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}