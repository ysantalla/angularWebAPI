using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class CurrencyViewModel
    {
        public long Id { get; set; } = 0;
        [Required]
        public string Name { get; set; }
        [Required]
        public string Symbol { get; set; }
        public bool IsDeleted { get; set; }
    }
}