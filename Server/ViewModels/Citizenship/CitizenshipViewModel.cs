using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class CitizenshipViewModel
    {
        public long Id { get; set; } = 0;
        [Required]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}