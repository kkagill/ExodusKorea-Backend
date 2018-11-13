using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ExodusKorea.Model.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string NickName { get; set; }

        public ICollection<VideoPostLike> VideoPostLikes { get; set; }
        public ICollection<CommentLike> CommentLikes { get; set; }
        public ICollection<CommentReplyLike> CommentReplyLikes { get; set; }
    }
}