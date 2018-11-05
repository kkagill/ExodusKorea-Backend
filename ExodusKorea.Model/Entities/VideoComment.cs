using System;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class VideoComment
    {
        public int VideoCommentId { get; set; }
        public string AuthorDisplayName { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int Likes { get; set; }

        public int NewVideoId { get; set; }
        public NewVideo NewVideo { get; set; }
        public ICollection<VideoCommentReply> VideoCommentReplies { get; set; }
    }
}