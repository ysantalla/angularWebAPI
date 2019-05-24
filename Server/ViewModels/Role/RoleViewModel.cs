using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class RoleViewModel
    {
        public long Id { get; set; } = 0;
        [Required]
        public string Name { get; set; }
    }
}