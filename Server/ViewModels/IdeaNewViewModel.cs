using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.ViewModels
{
    public class IdeaNewViewModel
    {
        public long? Id { get; set; }
        [Required]
        [StringLength(60, MinimumLength=5)]
        public string Title { get; set; }
        // TODO: 3 < length < 6
        [Required]
        public List<string> Tags { get; set; }
        [Required]
        [MinLength(100)]
        public string Article { get; set; }
    }
}