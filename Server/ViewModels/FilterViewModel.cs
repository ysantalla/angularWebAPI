using Server.Models.Enums;

namespace Server.ViewModels
{
    public class FilterViewModel
    {
        public Filter Filter { get; set; }
        public Period Period { get; set; }
        public long? TagId { get; set; }
        public long? LastIdeaId { get; set; }
        public int? TakeSize { get; set; }
    }
}