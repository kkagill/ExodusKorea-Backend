using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExodusKorea.Model.Entities
{
    public class VideoPostLike
    {
        public int VideoPostId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("VideoPostId")]
        public VideoPost VideoPost { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}