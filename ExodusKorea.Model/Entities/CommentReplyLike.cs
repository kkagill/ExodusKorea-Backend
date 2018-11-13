using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExodusKorea.Model.Entities
{
    public class CommentReplyLike
    {
        public long VideoCommentReplyId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("VideoCommentReplyId")]
        public VideoCommentReply VideoCommentReply { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}