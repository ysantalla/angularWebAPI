using Server.Models;
using System;

namespace Server.Models
{
    public class relIdeaTag : BaseModel
    {
        public long IdeaId { get; set; }
        public long TagId { get; set; }
        public Tag Tag { get; set; }
        public Idea Idea { get; set; }
    }
}