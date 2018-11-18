using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class VideoComment
    {
        public long VideoCommentId { get; set; }
        public string AuthorDisplayName { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int Likes { get; set; }
        public string UserId { get; set; }
        public string Country { get; set; }

        public int VideoPostId { get; set; }
        public VideoPost VideoPost { get; set; }
        public ICollection<VideoCommentReply> VideoCommentReplies { get; set; }
        public ICollection<CommentLike> CommentLikes { get; set; }
    }
}