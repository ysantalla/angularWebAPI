using Server.Enums;
using Server.Models;
using System;

namespace Server.Models
{
    public class relIdeaLike : BaseModel
    {
        public long IdeaId { get; set; }
        public Vote Vote { get; set; }
        public Idea Idea { get; set; }
    }
}