using System.ComponentModel.DataAnnotations;
using Server.Enums;

namespace Server.ViewModels
{
    public class LikeViewModel
    {
        public long ObjectId { get; set; }
        [EnumDataType(typeof(Vote))]
        public Vote Vote { get; set; }
    }
}