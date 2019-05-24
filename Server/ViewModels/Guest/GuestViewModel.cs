using System.ComponentModel.DataAnnotations;
using Server.ViewModels;

namespace Server.ViewModels
{
    public class GuestViewModel
    {
        public long Id { get; set; } = 0;

        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Identification { get; set; }
        [Required]
        public string Birthday { get; set; }

        [Required]
        public long CountryID { get; set; }

        [Required]
        public long CitizenshipID { get; set; }
        public bool IsDeleted { get; set; }
    }
}