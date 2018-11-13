using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExodusKorea.Model.Entities
{
    public class CommentLike
    {
        public long VideoCommentId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("VideoCommentId")]
        public VideoComment VideoComment { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}