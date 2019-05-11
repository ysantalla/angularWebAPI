using Server.Enums;
using Server.Models;
using System;

namespace Server.Models
{
    public class relCommentLike : BaseModel
    {
        public long CommentId { get; set; }
        public Vote Vote { get; set; }
        public Comment Comment { get; set; }
    }
}