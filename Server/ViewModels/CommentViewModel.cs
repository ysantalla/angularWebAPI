using Server.Enums;
using System;

namespace Server.ViewModels
{
    public class CommentViewModel
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public CreatorViewModel Creator { get; set; }
        public Vote CurrentUserLike { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}