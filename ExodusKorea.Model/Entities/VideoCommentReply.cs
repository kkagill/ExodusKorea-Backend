using System;

namespace ExodusKorea.Model.Entities
{
    public class VideoCommentReply
    {
        public int VideoCommentReplyId { get; set; }
        public string AuthorDisplayName { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int Likes { get; set; }

        public int VideoCommentId { get; set; }
        public VideoComment VideoComment { get; set; }
    }
}