using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class VideoCommentReply
    {
        public long VideoCommentReplyId { get; set; }
        public string AuthorDisplayName { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int Likes { get; set; }
        public string UserId { get; set; }
        public string RepliedTo { get; set; }
        public string Country { get; set; }
        public string IPAddress { get; set; }
        public bool IsSharer { get; set; }

        public long VideoCommentId { get; set; }
        public VideoComment VideoComment { get; set; }
        public ICollection<CommentReplyLike> CommentReplyLikes { get; set; }
    }
}